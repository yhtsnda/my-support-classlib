using Projects.Tool.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public abstract class AbstractCache : ICacheSettingable
    {
        int _expiredSeconds = 1200;

        public virtual void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            string def = TryGetValue(settingNodes, "expiredSeconds");
            if (def != null)
                _expiredSeconds = Int32.Parse(def);
        }

        public string CacheName
        {
            get;
            set;
        }

        public int ExpiredSeconds
        {
            get { return _expiredSeconds; }
        }

        public virtual void Set<T>(string key, T value)
        {
            Set(key, value, ExpiredSeconds);
        }

        public virtual void Set<T>(string key, T value, int expiredSeconds)
        {
            if (expiredSeconds <= 0)
                expiredSeconds = _expiredSeconds;
            SetInner(key, value, CurrentTime.AddSeconds(expiredSeconds));
        }

        public virtual void Set<T>(string key, T value, DateTime expiredTime)
        {
            SetInner(key, value, expiredTime);
        }

        protected abstract void SetInner<T>(string key, T value, DateTime expiredTime);

        public virtual void SetBatch<T>(IEnumerable<CacheItem<T>> items, int expiredSeconds)
        {
            if (expiredSeconds <= 0)
                expiredSeconds = _expiredSeconds;
            DateTime expriedTime = CurrentTime.AddSeconds(expiredSeconds);
            SetBatch(items, expriedTime);
        }

        public virtual void SetBatch<T>(IEnumerable<CacheItem<T>> items, DateTime expiredTime)
        {
            foreach (CacheItem<T> item in items)
            {
                SetInner(item.Key, item.Value, expiredTime);
            }
        }

        public virtual T Get<T>(string key)
        {
            var v = Get(typeof(T), key);
            if (v == null)
                return default(T);
            return (T)v;
        }

        public virtual object Get(Type type, string key)
        {
            object value = GetInner(type, key);
            TraceCache(key, value == null ? 0 : 1);
            return value;
        }

        protected virtual void TraceCache(string key, int missing)
        {
            if (ProfilerContext.Current.Enabled)
            {
                bool isLocal = this is HttpContextCache || this is AspnetCache || this is StaticCache;
                ProfilerContext.Current.Trace(isLocal ? "localcache" : "cache", String.Format("[{0}] @{3} {2}/1\r\n{1} ", GetType().Name, key, missing, CacheName));
            }
        }

        protected virtual void TraceCache(IEnumerable<string> keys, int missing)
        {
            if (ProfilerContext.Current.Enabled)
            {
                bool isLocal = this is HttpContextCache || this is AspnetCache || this is StaticCache;
                int count = keys.Count();
                ProfilerContext.Current.Trace(isLocal ? "localcache" : "cache", String.Format("[{0}] @{4} {2}/{3}\r\n{1} ", GetType().Name, String.Join(",", keys), count - missing, count, CacheName));
            }
        }

        protected virtual DateTime CurrentTime
        {
            get { return NetworkTime.Now; }
        }

        protected abstract object GetInner(Type type, string key);

        public virtual IEnumerable GetBatch(Type type, IEnumerable<string> keys)
        {
            IEnumerable<string> missingKeys;
            return GetBatch(type, keys, out missingKeys);
        }

        public virtual IEnumerable GetBatch(Type type, IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            ArrayList items = new ArrayList();
            List<string> missings = new List<string>();
            foreach (string key in keys)
            {
                object value = Get(type, key);
                if (value != null)
                    items.Add(value);
                else
                    missings.Add(key);
            }
            missingKeys = missings.ToArray();
            TraceCache(keys, missings.Count);
            return items;
        }

        public virtual IEnumerable<T> GetBatch<T>(IEnumerable<string> keys)
        {
            IEnumerable<string> missingKeys;
            return GetBatch<T>(keys, out missingKeys);
        }

        public virtual IEnumerable<T> GetBatch<T>(IEnumerable<string> keys, out IEnumerable<string> missingKeys)
        {
            List<T> items = new List<T>();
            List<string> missings = new List<string>();
            foreach (string key in keys)
            {
                T value = Get<T>(key);
                if (value != null)
                    items.Add(value);
                else
                    missings.Add(key);
            }
            missingKeys = missings.ToArray();
            TraceCache(keys, missings.Count);
            return items;
        }

        public virtual IEnumerable<CacheItemResult> GetBatchResult(Type type, IEnumerable<string> keys)
        {
            List<CacheItemResult> items = new List<CacheItemResult>();
            foreach (string key in keys)
            {
                object value = Get(type, key);
                items.Add(new CacheItemResult() { Key = key, Value = value, IsMissing = value == null });
            }
            TraceCache(keys, items.GetMissingKeys().Count());
            return items;
        }

        public virtual void Remove(Type type, string key)
        {
            RemoveInner(type, key);
        }

        protected abstract void RemoveInner(Type type, string key);

        public virtual bool Contains<T>(string key)
        {
            return Get<T>(key) != null;
        }

        internal static bool IsDefault<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        protected string TryGetValue(IEnumerable<SettingNode> settingNodes, string path)
        {
            return CacheUtil.TryGetValue(settingNodes, path);
        }

        protected SettingNode TryGetNode(IEnumerable<SettingNode> settingNodes, string path)
        {
            return CacheUtil.TryGetNode(settingNodes, path);
        }

        protected IEnumerable<SettingNode> TryGetNodes(IEnumerable<SettingNode> settingNodes, string path)
        {
            return CacheUtil.TryGetNodes(settingNodes, path);
        }

        internal static IList<SettingNode> GetSettingNodes(SettingNode settingNode)
        {
            List<SettingNode> settingNodes = new List<SettingNode>();
            if (settingNode != null && !settingNode.IsRoot)
                settingNodes.Add(settingNode);
            if (settingNode == null || !settingNode.IsRoot)
                settingNodes.Add(ToolSection.Instance.RootNode);
            return settingNodes;
        }
    }
}
