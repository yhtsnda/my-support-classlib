using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    /// <summary>
    /// 抽象支持二级缓存的缓存对象
    /// </summary>
    public class SecondaryCache : AbstractCache
    {
        ICache firstCache;
        ICache secondCache;

        public SecondaryCache()
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

            this.firstCache = firstCache;
            this.secondCache = secondCache;
        }

        public ICache FirstCache
        {
            get { return firstCache; }
            protected set { firstCache = value; }
        }

        public ICache SecondCache
        {
            get { return secondCache; }
            protected set { secondCache = value; }
        }

        public override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            firstCache = CacheUtil.TryCreateCache(settingNodes, "firstType");
            secondCache = CacheUtil.TryCreateCache(settingNodes, "secondType");
            if (firstCache == null)
                firstCache = new AspnetCache();

            if (secondCache == null)
                throw new MissConfigurationException(settingNodes, "secondType");

            if (firstCache.GetType() == secondCache.GetType())
                throw new ConfigurationException(String.Format("一二级缓存的类型不能一致，当前皆为 {0}。", firstCache.GetType().FullName));

            if (firstCache != this)
                CacheUtil.InitCache(firstCache, CacheUtil.GetCacheSettingNode(settingNodes, firstCache));
            if (secondCache != this)
                CacheUtil.InitCache(secondCache, CacheUtil.GetCacheSettingNode(settingNodes, secondCache));
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            OnSetFirstCache(key, value, expiredTime);
            secondCache.Set(key, value);
        }

        protected virtual void OnSetFirstCache<T>(string key, T value, DateTime expiredTime)
        {
            //丢弃时间，使用默认时间
            firstCache.Set(key, value);
        }

        protected override void RemoveInner(Type type, string key)
        {
            firstCache.Remove(type, key);
            secondCache.Remove(type, key);
        }

        protected override void TraceCache(string key, int missing)
        {
        }

        protected override void TraceCache(IEnumerable<string> keys, int missing)
        {
        }

        protected override object GetInner(Type type, string key)
        {
            object value = firstCache.Get(type, key);
            if (value != null)
                return value;

            value = secondCache.Get(type, key);
            if (value != null)
                OnSetFirstCache(key, value, CurrentTime.AddSeconds(ExpiredSeconds));
            return value;
        }
    }
}
