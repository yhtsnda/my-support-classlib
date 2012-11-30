using Castle.DynamicProxy;
using Nd.Tool;
using Nd.Tool.Profiler;
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
                throw new NotSupportedException(String.Format("类型 {0} 未进行定义，因此无法实现缓存扩展。", entityType.FullName));

            var pns = metadata.CacheRegionMembers.ToHashSet(o => o.Name);
            foreach (var region in regions)
            {
                if (String.IsNullOrEmpty(region.PropertyName))
                {
                    if (!metadata.IsCacheRegion)
                        throw new ArgumentException(String.Format("对象 {0} 未定义为缓存区域", metadata.EntityType.ToPrettyString()));
                }
                else if (!pns.Contains(region.PropertyName))
                {
                    throw new ArgumentException(String.Format("对象 {0} 属性 {1} 未定义为缓存区域", metadata.EntityType.ToPrettyString(), region.PropertyName));
                }
            }

            foreach (var type in providers)
            {
                AbstractCacheableRepositoryProvider<T> provider = (AbstractCacheableRepositoryProvider<T>)Nd.Tool.Reflection.FastActivator.Create(type);
                provider.Invocation = invocation;
                if (provider.IsMatch())
                {
                    var cacheData = provider.GetCacheData();
                    if (cacheData != null && IsValid(cacheData))
                    {
                        log.DebugFormat("{0}.{1} query cache hit. #{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, provider.CacheKey);
                        if (ProfilerContext.Current.Enabled)
                            ProfilerContext.Current.Trace("platform", String.Format("{0}.{1} query cache hit@Key:\r\n{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, ProfilerUtil.JsonFormat(provider.CacheKey)));
                        //处理缓存
                        provider.ProcessCache(cacheData);
                        return;
                    }

                    log.DebugFormat("{0}.{1} query cache missing. #{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, provider.CacheKey);
                    if (ProfilerContext.Current.Enabled)
                        ProfilerContext.Current.Trace("platform", String.Format("{0}.{1} query cache missing@Key:\r\n{2}", invocation.Method.DeclaringType.ToPrettyString(), invocation.Method.Name, ProfilerUtil.JsonFormat(provider.CacheKey)));
                    //处理原始
                    provider.ProcessSource();

                    return;
                }
            }
            invocation.Proceed();
        }

        bool IsValid(IQueryTimestamp cacheData)
        {
            ICache dependCache = CacheManager.GetCacher("cache:region");
            var values = dependCache.GetBatch<long>(regions.Select(o => o.GetCacheRegionKey()));
            return values.Count() == 0 || values.All(o => o < cacheData.Timestamp);
        }
    }
}
