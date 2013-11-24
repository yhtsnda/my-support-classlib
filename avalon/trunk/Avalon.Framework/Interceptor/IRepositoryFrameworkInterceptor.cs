using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    /// <summary>
    /// 仓储拦截器
    /// </summary>
    public interface IRepositoryFrameworkInterceptor
    {
        void PreUpdate(object entity);

        void PreCreate(object entity);

        void PreDelete(object entity);

        void PostUpdate(object entity);

        void PostCreate(object entity);

        void PostDelete(object entity);

        void PostLoad(object entity);
    }

    /// <summary>
    /// 仓储拦截器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryFrameworkInterceptor<TEntity> : IRepositoryFrameworkInterceptor where TEntity : class
    {
        void PreUpdate(TEntity entity);

        void PreCreate(TEntity entity);

        void PreDelete(TEntity entity);

        void PostUpdate(TEntity entity);

        void PostCreate(TEntity entity);

        void PostDelete(TEntity entity);

        void PostLoad(TEntity entity);
    }
}
