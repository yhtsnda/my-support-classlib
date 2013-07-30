using Castle.DynamicProxy;
using Projects.Tool;
using Projects.Tool.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Framework
{
    interface ICacheableInterceptor : IInterceptor
    {
        void SetRegions(List<CacheRegion> depends);

        void SetCacheKey(string cacheKey);
    }

    internal class CacheableRepositoryInterceptor<T> : ICacheableInterceptor
        where T : class
    {
        ILog log = LogManager.GetLogger<CacheableRepositoryInterceptor<T>>();
        string cacheKey = null;

        List<CacheRegion> regions;
        Type[] providers = new Type[]{
            typeof(EntityFindAllCacheableRepositoryProvider<T>),
            typeof(EntityPagingCacheableRepositoryProvider<T>),
            typeof(EntityFindOneCacheableRepositoryProvider<T>)//,
            //typeof(CommonCacheableRepositoryProvider<T>)
        };

        public void SetRegions(List<CacheRegion> regions)
        {
            this.regions = regions;
            //验证缓存区域设置
            foreach (var region in regions)
            {
                var metadata = RepositoryFramework.GetDefineMetadata(region.EntityType);
                if (metadata == null)
                    throw new NotSupportedException(String.Format("类型 {0} 未进行定义，因此无法实现仓储扩展。", region.EntityType.FullName));
                metadata.CheckCacheRegions(new[] { region });
            }
        }

        public void Intercept(IInvocation invocation)
        {
            var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(invocation.TargetType);
            var metadata = RepositoryFramework.GetDefineMetadata(entityType);

            if (metadata == null)
                throw new NotSupportedException(String.Format("类型 {0} 未进行定义，因此无法实现仓储扩展。", entityType.FullName));

            foreach (var type in providers)
            {
                AbstractCacheableRepositoryProvider<T> provider = (AbstractCacheableRepositoryProvider<T>)Projects.Tool.Reflection.FastActivator.Create(type);
                provider.Invocation = invocation;
                if (provider.IsMatch())
                {
                    provider.CacheKey = cacheKey;
                    var regionStr = String.Join(",", regions.Select(o => o.CacheKey));

                    var cacheData = provider.GetCacheData();
                    if (cacheData != null && IsCacheValid(metadata, cacheData))
                    {
                        log.DebugFormat("{0}.{1} query cache hit. #{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, provider.CacheKey);
                        if (ProfilerContext.Current.Enabled)
                            ProfilerContext.Current.Trace("platform", String.Format("hit  {1}\r\n{0}", ProfilerUtil.JsonFormat(provider.CacheKey), regionStr));

                        //处理缓存
                        using (MonitorContext.Repository(invocation.Method, true))
                            provider.ProcessCache(cacheData);
                        return;
                    }

                    //处理原始
                    var hasSource = provider.ProcessSource();

                    log.DebugFormat("{0}.{1} query cache missing. #{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, provider.CacheKey);
                    if (ProfilerContext.Current.Enabled)
                        ProfilerContext.Current.Trace("platform", String.Format("missing  {1}{2}\r\n{0}", ProfilerUtil.JsonFormat(provider.CacheKey), regionStr, hasSource ? "" : " for null"));
                    return;
                }
            }

            invocation.Proceed();
        }

        bool IsCacheValid(ClassDefineMetadata metadata, IQueryTimestamp cacheData)
        {
            ICache dependCache = RepositoryFramework.GetCacher(metadata, "cache:region");
            var values = dependCache.GetBatch<long>(regions.Select(o => o.CacheKey));
            return values.Count() == 0 || values.All(o => o < cacheData.Timestamp);
        }


        public void SetCacheKey(string cacheKey)
        {
            this.cacheKey = cacheKey;
        }
    }
}
