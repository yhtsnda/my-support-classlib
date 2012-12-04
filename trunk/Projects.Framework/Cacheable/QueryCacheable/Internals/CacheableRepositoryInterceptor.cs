using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Castle.DynamicProxy;
using Projects.Tool;

namespace Projects.Framework
{
    interface ICacheableInterceptor : IInterceptor
    {
        void SetRegions(List<CacheRegion> depends);
    }

    internal class CacheableRepositoryInterceptor<T> : ICacheableInterceptor
        where T : class
    {
        ILog log = LogManager.GetLogger<CacheableRepositoryInterceptor<T>>();

        List<CacheRegion> regions;
        Type[] providers = new Type[]{
            typeof(EntityFindAllCacheableRepositoryProvider<T>),
            typeof(EntityPagingCacheableRepositoryProvider<T>),
            typeof(EntityFindOneCacheableRepositoryProvider<T>),
            typeof(CommonCacheableRepositoryProvider<T>)
        };

        public void SetRegions(List<CacheRegion> regions)
        {
            this.regions = regions;
        }

        public void Intercept(IInvocation invocation)
        {
            var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(invocation.TargetType);
            var metadata = RepositoryFramework.GetDefineMetadata(entityType);

            if (metadata == null)
                throw new NotSupportedException(String.Format("类型 {0} 未进行定义，因此无法实现仓储扩展。", entityType.FullName));

            //验证缓存区域设置
            metadata.CheckCacheRegions(regions);

            foreach (var type in providers)
            {
                AbstractCacheableRepositoryProvider<T> provider = (AbstractCacheableRepositoryProvider<T>)
                    Projects.Tool.Reflection.FastActivator.Create(type);
                provider.Invocation = invocation;
                if (provider.IsMatch())
                {
                    var regionStr = String.Join(",", regions.Select(o => o.CacheKey));

                    var cacheData = provider.GetCacheData();
                    if (cacheData != null && IsCacheValid(metadata, cacheData))
                    {
                        log.DebugFormat("{0}.{1} query cache hit. #{2}", 
                            invocation.Method.DeclaringType.ToPrettyString(), 
                            invocation.Method.Name, provider.CacheKey);

                        //处理缓存
                        provider.ProcessCache(cacheData);
                        return;
                    }

                    //处理原始
                    var hasSource = provider.ProcessSource();

                    log.DebugFormat("{0}.{1} query cache missing. #{2}", 
                        invocation.Method.DeclaringType.ToPrettyString(), 
                        invocation.Method.Name, provider.CacheKey);
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
    }
}
