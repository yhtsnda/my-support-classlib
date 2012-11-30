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
    internal class DefaultRepositoryCacheable<T> : IRepositoryCacheable<T>, IEntityDefineSupport<T>
        where T : class,IRepository
    {
        List<CacheRegion> depends = new List<CacheRegion>();
        T resository;

        public DefaultRepositoryCacheable(T resository)
        {
            this.resository = resository;
        }

        public T Proxy()
        {
            var entityType = ReflectionHelper.GetEntityTypeFromRepositoryType(typeof(T));

            var type = typeof(CacheableRepositoryInterceptor<>).MakeGenericType(entityType);
            var interceptor = Nd.Tool.Reflection.FastActivator.Create(type);
            ((ICacheableInterceptor)interceptor).SetRegions(depends);

            T p = ProxyGeneratorUtil.Instance.CreateInterfaceProxyWithTarget<T>(resository, (IInterceptor)interceptor);
            return p;
        }

        public IRepositoryCacheable<T> Depend<TEntity>()
        {
            depends.Add(CacheRegion.Create<TEntity>());
            return this;
        }

        public IRepositoryCacheable<T> Depend<TEntity>(Expression<Func<TEntity, object>> property, object value)
        {
            depends.Add(CacheRegion.Create<TEntity>(property, value));
            return this;
        }

        IRepositoryCacheable<T> IEntityDefineSupport<T>.Depend(List<CacheRegion> cacheDepends, object entity)
        {
            depends.AddRange(cacheDepends);
            foreach (var depend in depends)
            {
                if (depend.ValueFunc != null)
                    depend.Value = depend.ValueFunc(entity);
            }
            return this;
        }
    }
}
