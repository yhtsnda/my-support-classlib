using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using Avalon.Profiler;

namespace Avalon.Utility
{
    /// <summary>
    /// 缓存的抽象类
    /// </summary>
    public abstract class AbstractCache : ICacheImplementor
    {
        const string StatCache = "cache";
        const string StatEntityCache = "entitycache";

        int expiredSeconds = 1200;
        bool emptySupport;
        bool isInner = false;
        bool inited = false;

        /// <summary>
        /// 获取或设置缓存的名称
        /// </summary>
        public virtual string CacheName { get; set; }

        /// <summary>
        /// 缓存的过期秒数
        /// </summary>
        public virtual int ExpiredSeconds
        {
            get;
            set;
        }

        /// <summary>
        /// 是否支持空值缓存
        /// </summary>
        public virtual bool EmptySupport
        {
            get { return emptySupport; }
            set
            {
                if (IsInited)
                    throw new ArgumentException("在初始化后无法变更设置 EmptySupport。");
                emptySupport = value;
            }
        }

        public T Get<T>(string key)
        {
            return GetResult(typeof(T), key).GetValue<T>();
        }

        public object Get(Type type, string key)
        {
            return GetResult(type, key).GetValue();
        }

        public void Set(string key, object value)
        {
            Set(key, value, ExpiredSeconds);
        }

        public void Set(string key, object value, int expiredSeconds)
        {
            if (expiredSeconds <= 0)
                expiredSeconds = this.expiredSeconds;
            Set(key, value, CurrentTime.AddSeconds(expiredSeconds));
        }

        public void Set(string key, object value, DateTime expiredTime)
        {
            var cacheItem = new CacheItem(key, value);
            SetBatch(new List<CacheItem> { cacheItem }, expiredTime);
        }

        public IEnumerable GetBatch(Type type, IEnumerable<string> keys)
        {
            return GetBatchResult(type, keys).Where(o => o.HasData).Select(o => o.GetValue()).ToList();
        }

        public IEnumerable<T> GetBatch<T>(IEnumerable<string> keys)
        {
            return GetBatchResult(typeof(T), keys).Where(o => o.HasData).Select(o => o.GetValue<T>()).ToList();
        }

        public IList<CacheItemResult> GetBatchResult(Type type, IEnumerable<string> keys)
        {
            EnsureInit();

            // 从上下文获取
            var results = this.GetContext(type, keys);
            int contextHits = results.GetHitKeys().Count();

            // 从缓存获取上下文缺失的对象
            var innerResults = GetBatchResultInner(type, results.GetMissingKeys());
            // 处理空值
            if (EmptySupport)
            {
                foreach (var result in innerResults)
                {
                    if (result.IsEmpty)
                        result.SetEmpty();
                }
            }

            // 将缓存获取到的对象写入上下文
            var innerHits = innerResults.GetHitItems().ToCacheItems();
            if (innerHits.Count() > 0)
            {
                this.SetContext(innerHits);

                //合并数据
                if (results.Count == 1)
                    results[0].Merge(innerResults[0]);
                else
                    results.Merge(innerResults);
            }

            // 写跟踪信息
            var keyCount = keys.Count();
            var hitCount = results.GetHitKeys().Count();

            TraceCache(keys, contextHits, results.GetHitKeys().Count());

            if (!isInner && StatService.CheckEnabled(StatCache))
            {
                StatService.IncrementInt64(StatCache, "read");
                StatService.AddInt64(StatCache, "read_item_count", keyCount);
                StatService.AddInt64(StatCache, "read_item_hit", hitCount);
            }

            if (!isInner && StatService.CheckEnabled(StatEntityCache))
            {
                StatService.Increment(StatEntityCache, CacheName + ":read");
                StatService.Add(StatEntityCache, CacheName + ":read_item_count", keyCount);
                StatService.Add(StatEntityCache, CacheName + ":read_item_hit", hitCount);
            }

            return results;
        }


        public void SetBatch(IEnumerable<CacheItem> items)
        {
            SetBatch(items, ExpiredSeconds);
        }

        public void SetBatch(IEnumerable<CacheItem> items, int expiredSeconds)
        {
            if (expiredSeconds <= 0)
                expiredSeconds = this.expiredSeconds;
            DateTime expriedTime = CurrentTime.AddSeconds(expiredSeconds);
            SetBatch(items, expriedTime);
        }

        public void SetBatch(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            EnsureInit();

            List<CacheItem> outputs = new List<CacheItem>();

            //处理空值
            int count = 0;
            foreach (var item in items)
            {
                count++;
                if (item.Value == null)
                {
                    if (!EmptySupport)
                        continue;

                    item.Value = EmptyData.Value;
                }
                outputs.Add(item);
            }
            if (outputs.Count > 0)
            {
                // 保存到上下文
                this.SetContext(items);
                // 保存到缓存
                SetBatchInner(outputs, expiredTime);
            }

            // 写跟踪信息
            if (!isInner && count > 0)
            {
                if (StatService.CheckEnabled(StatCache))
                {
                    StatService.IncrementInt64(StatCache, "write");
                    StatService.AddInt64(StatCache, "write_item_count", count);
                }

                if (StatService.CheckEnabled(StatEntityCache))
                {
                    StatService.Increment(StatEntityCache, CacheName + ":write");
                    StatService.Add(StatEntityCache, CacheName + ":write_item_count", count);
                }
            }
        }

        public void Remove(Type type, string key)
        {
            EnsureInit();

            this.RemoveContext(type, key);
            RemoveInner(type, key);

            if (!isInner && StatService.CheckEnabled(StatCache))
                StatService.IncrementInt64(StatCache, "remove");

            if (!isInner && StatService.CheckEnabled(StatEntityCache))
                StatService.Increment(StatEntityCache, CacheName + ":remove");
        }

        public bool Contains(Type type, string key)
        {
            EnsureInit();
            return this.ContainsContext(type, key) || ContainsInner(type, key);
        }

        public CacheItemResult GetResult(Type type, string key)
        {
            return GetBatchResult(type, new string[] { key }).First();
        }

        protected virtual DateTime CurrentTime
        {
            get { return NetworkTime.Now; }
        }

        protected void EnsureInit()
        {
            if (!inited)
                InitCache();
        }

        protected abstract IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys);

        protected abstract void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime);

        protected abstract void RemoveInner(Type type, string key);

        protected abstract bool ContainsInner(Type type, string key);

        protected bool ContainsByResult(Type type, string key)
        {
            return GetResult(type, key).IsHit;
        }

        #region ICacheImplementor
        protected virtual bool IsLocal
        {
            get { return true; }
        }

        bool ICacheImplementor.IsLocal
        {
            get { return IsLocal; }
        }

        bool ICacheImplementor.IsInited
        {
            get { return inited; }
        }

        void ICacheImplementor.InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            InitSetting(settingNodes);
        }

        void ICacheImplementor.InitCache()
        {
            InitCache();
        }

        void ICacheImplementor.SetInnerCache()
        {
            isInner = true;
        }

        protected bool IsInited
        {
            get { return inited; }
        }

        protected virtual void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            this.TrySetSetting(settingNodes, ConfigurationName, "expiredSeconds", (o) => o.ExpiredSeconds);
            this.TrySetSetting(settingNodes, ConfigurationName, "emptySupport", (o) => o.EmptySupport);
        }

        protected string ConfigurationName
        {
            get
            {
                var name = GetType().Name;
                return name.Substring(0, 1).ToLower() + name.Substring(1);
            }
        }

        protected virtual void InitCache()
        {
            if (String.IsNullOrEmpty(CacheName))
                throw new ArgumentException("缓存名称不能为空。");
            inited = true;
        }
        #endregion

        protected virtual void TraceCache(IEnumerable<string> keys, int contextHit, int hit)
        {
            if (ProfilerContext.Current.Enabled)
            {
                int count = keys.Count();
                string cacheGroup = "remotecache";
                if (contextHit == count)
                    cacheGroup = "contextcache";
                else
                    cacheGroup = IsLocal ? "localcache" : "remotecache";

                if (cacheGroup == "contextcache")
                    ProfilerContext.Current.Trace(cacheGroup,
                        String.Format("[{0}] @{1} {2}/{3}\r\n {4} ", GetType().Name, CacheName, contextHit, count, String.Join(",", keys)));
                else
                    ProfilerContext.Current.Trace(cacheGroup,
                        String.Format("[{0}] @{1} {2}+{3}/{4}\r\n {5} ", GetType().Name, CacheName, hit - contextHit, contextHit, count, String.Join(",", keys)));
            }
        }


    }
}
