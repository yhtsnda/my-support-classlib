using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 将普通的缓存转为依赖缓存
    /// </summary>
    public class DependableCache : AbstractCache
    {
        ICacheImplementor innerCache;
        ICacheDependProvider dependProvider;

        protected DependableCache()
        {
        }

        public DependableCache(ICache innerCache, ICacheDependProvider dependProvider)
        {
            this.innerCache = (ICacheImplementor)innerCache;
            this.dependProvider = dependProvider;

            if (innerCache == null)
                throw new ArgumentNullException("cache");
            if (dependProvider == null)
                throw new ArgumentNullException("dependProvider");
        }

        public ICache InnerCache
        {
            get { return innerCache; }
            set { innerCache = (ICacheImplementor)value; }
        }

        public ICacheDependProvider DependProvider
        {
            get { return dependProvider; }
            set { dependProvider = value; }
        }

        protected override bool IsLocal
        {
            get { return innerCache.IsLocal; }
        }

        protected override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            var c = ToolSettingUtil.TryCreateInstance<ICacheImplementor>(settingNodes, ConfigurationName, "innerType");
            if (c != null)
                innerCache = c;

            var p = ToolSettingUtil.TryCreateInstance<ICacheDependProvider>(settingNodes, ConfigurationName, "dependProvider");
            if (p != null)
                dependProvider = p;

            if (innerCache != null)
                innerCache.InitSetting(ToolSettingUtil.GetInstanceSettingNodes(settingNodes, innerCache));

            if (dependProvider != null)
                dependProvider.InitSetting(ToolSettingUtil.GetInstanceSettingNodes(settingNodes, dependProvider));
        }

        protected override void InitCache()
        {
            base.InitCache();

            if (innerCache == null)
                throw new ArgumentNullException("innerCache");
            if (dependProvider == null)
                throw new ArgumentNullException("dependProvider");

            innerCache.CacheName = CacheName;
            innerCache.InitCache();
            dependProvider.Init();
        }

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            var dependType = GetDependableDataType(type);
            var results = innerCache.GetBatchResult(dependType, keys);

            var hitKeys = results.Where(o => o.IsHit && !o.IsContextHit).GetHitKeys().ToList();

            Dictionary<string, CacheDependResult> dependDic = new Dictionary<string, CacheDependResult>();
            if (hitKeys.Count > 0)
                dependDic = dependProvider.GetBatchDependResult(hitKeys).ToDictionary(o => o.Key);

            foreach (var result in results)
            {
                if (result.IsHit)
                {
                    var value = result.GetValue();
                    //可能非 IDependableData 类型，直接丢弃。
                    if (value is IDependableData)
                    {
                        var data = (IDependableData)value;
                        if (result.IsContextHit || dependDic[result.Key].Valid(data))
                        {
                            result.Value = data.Data;
                            continue;
                        }
                    }
                    result.SetNull();
                }
            }
            return results;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            foreach (var item in items)
            {
                if (item.Value != null)
                {
                    var dependType = GetDependableDataType(item.Value.GetType());
                    var data = (IDependableData)FastActivator.Create(dependType);
                    data.Data = item.Value;

                    item.Value = data;
                }
            }
            innerCache.SetBatch(items, expiredTime);
        }

        protected override void RemoveInner(Type type, string key)
        {
            var dependType = GetDependableDataType(type);
            innerCache.Remove(dependType, key);
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return ContainsByResult(type, key);
        }

        protected override void TraceCache(IEnumerable<string> keys, int contextHit, int hit)
        {

        }

        Type GetDependableDataType(Type type)
        {
            return typeof(DependableData<>).MakeGenericType(type);
        }
    }
}
