using Avalon.Utility;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 仓储接口的缓存支持实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DefaultRepositoryCacheable<T> : IRepositoryCacheable<T>
        where T : class,IRepository
    {
        List<CacheRegion> depends = new List<CacheRegion>();
        T resository;
        string cacheKey;

        public DefaultRepositoryCacheable(T resository)
        {
            this.resository = resository;
            if (resository == null)
                throw new ArgumentNullException("resository", String.Format("类型 {0} 的仓储并未实现。", typeof(T).FullName));
        }

        public DefaultRepositoryCacheable(T resository, string cacheKey)
            : this(resository)
        {
            this.cacheKey = cacheKey;
        }

        public string CacheKey
        {
            get { return cacheKey; }
        }

        public T Proxy()
        {
            var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(typeof(T));

            var type = typeof(CacheableRepositoryInterceptor<>).MakeGenericType(entityType);
            var interceptor = FastActivator.Create(type);
            ICacheableInterceptor ci = (ICacheableInterceptor)interceptor;
            ci.SetRegions(depends);
            ci.SetCacheKey(cacheKey);

            ((ICacheableInterceptor)interceptor).SetRegions(depends);

            T p = ProxyProvider.Generator.CreateInterfaceProxyWithTarget<T>(resository, (IInterceptor)interceptor);
            return p;
        }

        public IRepositoryCacheable<T> Depend<TRegionEntity>()
        {
            depends.Add(CacheRegion.Create<TRegionEntity>());
            return this;
        }

        public IRepositoryCacheable<T> Depend<TRegionEntity>(string regionName, object value)
        {
            depends.Add(CacheRegion.Create<TRegionEntity>(regionName, value));
            return this;
        }

        public IRepositoryCacheable<T> Depend(IEnumerable<CacheRegion> regions)
        {
            depends.AddRange(regions);
            return this;
        }
    }
}
