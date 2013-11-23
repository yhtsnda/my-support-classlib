using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 抽象支持二级缓存的缓存对象
    /// </summary>
    public class SecondaryCache : AbstractCache
    {
        ICacheImplementor firstCache;
        ICacheImplementor secondCache;

        protected SecondaryCache()
        {
        }

        public SecondaryCache(ICache firstCache, ICache secondCache)
        {
            if (firstCache == null)
                throw new ArgumentNullException("firstCache");
            if (secondCache == null)
                throw new ArgumentNullException("secondCache");
            if (firstCache.GetType() == secondCache.GetType())
                throw new ConfigurationException(String.Format("一二级缓存的类型不能一致，当前皆为 {0}。", firstCache.GetType().FullName));

            this.firstCache = (ICacheImplementor)firstCache;
            this.secondCache = (ICacheImplementor)secondCache;
        }

        protected override bool IsLocal
        {
            get
            {
                return secondCache.IsLocal;
            }
        }

        public ICache FirstCache
        {
            get { return firstCache; }
            protected set { firstCache = (ICacheImplementor)value; }
        }

        public ICache SecondCache
        {
            get { return secondCache; }
            protected set { secondCache = (ICacheImplementor)value; }
        }

        protected override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            var fc = ToolSettingUtil.TryCreateInstance<ICacheImplementor>(settingNodes, ConfigurationName, "firstCache");
            if (fc != null)
                firstCache = fc;

            var sc = ToolSettingUtil.TryCreateInstance<ICacheImplementor>(settingNodes, ConfigurationName, "secondCache");
            if (sc != null)
                secondCache = sc;

            if (firstCache != this)
                firstCache.InitSetting(ToolSettingUtil.GetInstanceSettingNodes(settingNodes, firstCache));
            if (secondCache != this)
                secondCache.InitSetting(ToolSettingUtil.GetInstanceSettingNodes(settingNodes, secondCache));
        }

        protected override void InitCache()
        {
            if (firstCache == null)
                throw new ArgumentNullException("firstCache");
            if (secondCache == null)
                throw new ArgumentNullException("secondCache");

            if (firstCache.GetType() == secondCache.GetType())
                throw new ArgumentException("一二级缓存的类型不能一致，当前皆为 " + firstCache.GetType().FullName);

            firstCache.CacheName = CacheName;
            secondCache.CacheName = CacheName;

            firstCache.SetInnerCache();
            secondCache.SetInnerCache();

            firstCache.InitCache();
            secondCache.InitCache();

            base.InitCache();
        }

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            var firstResult = firstCache.GetBatchResult(type, keys);
            var missingKeys = firstResult.GetMissingKeys().ToList();
            if (missingKeys.Count > 0)
            {
                var secondResult = secondCache.GetBatchResult(type, missingKeys);
                //赋值一级缓存

                firstCache.SetBatch(secondResult.ToCacheItems(), ExpiredSeconds);
                firstResult.Merge(secondResult);
            }

            return firstResult;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            secondCache.SetBatch(items, expiredTime);
            firstCache.SetBatch(items, expiredTime);
        }

        protected override void RemoveInner(Type type, string key)
        {
            secondCache.Remove(type, key);
            firstCache.Remove(type, key);
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return firstCache.Contains(type, key) || secondCache.Contains(type, key);
        }

        protected override void TraceCache(IEnumerable<string> keys, int contextHit, int hit)
        {

        }
    }
}
