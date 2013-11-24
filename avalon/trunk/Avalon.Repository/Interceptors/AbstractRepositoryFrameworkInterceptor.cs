using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
{
    public abstract class AbstractRepositoryFrameworkInterceptor : IRepositoryFrameworkInterceptor
    {
        public virtual void PreUpdate(object entity)
        {
        }

        public virtual void PreCreate(object entity)
        {
        }

        public virtual void PreDelete(object entity)
        {
        }

        public virtual void PostUpdate(object entity)
        {
        }

        public virtual void PostCreate(object entity)
        {
        }

        public virtual void PostDelete(object entity)
        {
        }

        public virtual void PostLoad(object entity)
        {
        }
    }
    /// <summary>
    /// 仓储系统的拦截器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractRepositoryFrameworkInterceptor<TEntity> : IRepositoryFrameworkInterceptor<TEntity> where TEntity : class
    {
        public virtual void PreUpdate(TEntity entity)
        {
        }

        public virtual void PreCreate(TEntity entity)
        {
        }

        public virtual void PreDelete(TEntity entity)
        {
        }

        public virtual void PostUpdate(TEntity entity)
        {
        }

        public virtual void PostCreate(TEntity entity)
        {
        }

        public virtual void PostDelete(TEntity entity)
        {
        }

        public virtual void PostLoad(TEntity entity)
        {
        }

        void IRepositoryFrameworkInterceptor.PreUpdate(object entity)
        {
            PreUpdate((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PreCreate(object entity)
        {
            PreCreate((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PreDelete(object entity)
        {
            PreDelete((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PostUpdate(object entity)
        {
            PostUpdate((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PostCreate(object entity)
        {
            PostCreate((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PostDelete(object entity)
        {
            PostDelete((TEntity)entity);
        }

        void IRepositoryFrameworkInterceptor.PostLoad(object entity)
        {
            PostLoad((TEntity)entity);
        }
    }
}
