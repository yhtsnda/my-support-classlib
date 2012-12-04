using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 仓储接口的缓存支持实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DefaultRepositoryCacheable<T> : IRepositoryCacheable<T> where T : class,IRepository
    {
        private List<CacheRegion> depends = new List<CacheRegion>();
        private T resository;

        public DefaultRepositoryCacheable(T resository)
        {
            this.resository = resository;
        }

        public T Proxy()
        {
            var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(typeof(T));

            var type = typeof(CacheableRepositoryInterceptor<>).MakeGenericType(entityType);
            var interceptor = Projects.Tool.Reflection.FastActivator.Create(type);
            ((ICacheableInterceptor)interceptor).SetRegions(depends);

            T p = ProxyGeneratorUtil.Instance.CreateInterfaceProxyWithTarget<T>(resository, (IInterceptor)interceptor);
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
