using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    /// <summary>
    /// 支持分区的仓储抽象类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IShardSessionFactory sessionFactory;

        public AbstractRepository()
        {
            sessionFactory = RepositoryFramework.GetSessionFactory(typeof(TEntity));
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory", String.Format("类型 {0} 未配置 IShardSessionFactory", typeof(TEntity).FullName));
        }

        protected virtual IShardSessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

        public virtual IShardSession<TEntity> OpenSession(TEntity entity)
        {
            return OpenSession(GetShardParams(entity));
        }

        public virtual IShardSession<TEntity> OpenSession(ISpecification<TEntity> spec)
        {
            return OpenSession(spec.ShardParams);
        }

        public virtual IShardSession<TEntity> OpenSession(ShardParams shardParams)
        {
            return sessionFactory.OpenSession<TEntity>(shardParams);
        }

        protected abstract ShardParams GetShardParams(TEntity entity);

        public virtual void Create(TEntity entity)
        {
            OpenSession(entity).Create(entity);
        }

        public virtual void Create(TEntity entity, object id)
        {
            // TODO: delete this method
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            OpenSession(entity).Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            OpenSession(entity).Delete(entity);
        }

        public virtual void SessionEvict(TEntity entity)
        {
            OpenSession(entity).SessionEvict(entity);
        }

        public virtual bool SessionContains(TEntity entity)
        {
            return OpenSession(entity).SessionContains(entity);
        }

        public virtual TEntity Get(ShardParams shardParams, object id)
        {
            return OpenSession(shardParams).Get(id);
        }

        public virtual IList<TEntity> GetList(ShardParams shardParams, IEnumerable ids)
        {
            if (!ids.GetEnumerator().MoveNext())
                return new List<TEntity>();

            return OpenSession(shardParams).GetList(ids);
        }

        public virtual TEntity FindOne(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).FindOne(spec);
        }

        public virtual IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).FindAll(spec);
        }

        public virtual PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).FindPaging(spec);
        }

        public virtual ISpecification<TEntity> CreateSpecification(ShardParams shardParams)
        {
            return CreateSpecification().Shard(shardParams);
        }

        public virtual ISpecification<TEntity> CreateSpecification()
        {
            return sessionFactory.SpecificationProvider.CreateSpecification<TEntity>();
        }


        public virtual int Count(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).Count(spec);
        }
    }
}
