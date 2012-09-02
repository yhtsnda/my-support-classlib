using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public class SecondaryCache : AbstractCache
    {
        private ICache mFirstCache;
        private ICache mSecondCache;

        public  ICache FirstCache
        {
            get { return mFirstCache; }
            protected set { mFirstCache = value; }
        }

        public  ICache SecondCache
        {
            get { return mSecondCache; }
            protected set { mSecondCache = value; }
        }

        public override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            mFirstCache = CacheUtil.TryCreateCache(settingNodes, "firstType");
            mSecondCache = CacheUtil.TryCreateCache(settingNodes, "secondType");
            if(mFirstCache == null)
                mFirstCache = new AspnetCache();
            if(mSecondCache == null)
                throw  new MissConfigurationException(settingNodes, "secondType");

            if(mFirstCache.GetType() == SecondCache.GetType())
                throw new ConfigurationException(String.Format("一二级缓存的类型不能一致，当前皆为 {0}。", mFirstCache.GetType().FullName));

            if (mFirstCache != this)
                CacheUtil.InitCache(mFirstCache, CacheUtil.GetCacheSettingNode(settingNodes, mFirstCache));
            if(mSecondCache != this)
                CacheUtil.InitCache(mSecondCache,CacheUtil.GetCacheSettingNode(settingNodes,mSecondCache));
        }

        protected override void SetInner<T>(string key, T value, DateTime expiredTime)
        {
            OnSetFirstCache(key, value, expiredTime);
            mSecondCache.Set(key, value);
        }

        protected override T GetInner<T>(string key)
        {
            T value = mFirstCache.Get<T>(key);
            if (!IsDefault(value))
                return value;

            value = mSecondCache.Get<T>(key);
            if (!IsDefault(value))
                OnSetFirstCache(key, value, CurrentTime.AddSeconds(ExpiredSeconds));
            return value;
        }

        protected override void RemoveInner(string key)
        {
            throw new NotImplementedException();
        }

        protected  virtual  void OnSetFirstCache<T>(string key, T value, DateTime expiredTime)
        {
            //丢弃缓存的时间
            mFirstCache.Set(key, value);
        }
    }
}
