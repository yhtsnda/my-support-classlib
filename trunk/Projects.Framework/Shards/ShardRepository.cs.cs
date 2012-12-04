using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Pager;
using Projects.Framework.Shards;

namespace Projects.Framework
{
    /// <summary>
    /// 支持分区的仓储抽象类
    /// </summary>
    public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IShardSessionFactory mSessionFactory;

        protected abstract ShardParams GetShardParams(TEntity entity);
        
        protected IShardSessionFactory SessionFactory
        {
            get { return mSessionFactory; }
        }

        public AbstractRepository()
        {
            mSessionFactory = RepositoryFramework.GetSessionFactory(typeof(TEntity));
            if (mSessionFactory == null)
                throw new ArgumentNullException("sessionFactory", 
                    String.Format("类型 {0} 未配置 IShardSessionFactory", typeof(TEntity).FullName));
        }

        protected IShardSession<TEntity> OpenSession(TEntity entity)
        {
            return OpenSession(GetShardParams(entity));
        }

        public IShardSession<TEntity> OpenSession(ISpecification<TEntity> spec)
        {
            return OpenSession(spec.ShardParams);
        }

        public IShardSession<TEntity> OpenSession(ShardParams shardParams)
        {
            return mSessionFactory.OpenSession<TEntity>(shardParams);
        }

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

        public virtual PagedList<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).FindPaging(spec);
        }

        public virtual ISpecification<TEntity> CreateSpecification(ShardParams shardParams)
        {
            return CreateSpecification().Shard(shardParams);
        }

        public virtual ISpecification<TEntity> CreateSpecification()
        {
            return mSessionFactory.SpecificationProvider.CreateSpecification<TEntity>();
        }


        public virtual int Count(ISpecification<TEntity> spec)
        {
            return OpenSession(spec).Count(spec);
        }

        public ISpecification<TEntity> CreateSpecification(object searchObj, int? pageIndex = -1, int? pageSize = 12)
        {
            throw new NotImplementedException();
        }

        ActionResult<ResultKey, TEntity> IRepository<TEntity>.Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        ActionResult<ResultKey, object> IRepository<TEntity>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        ActionResult<ResultKey, object> IRepository<TEntity>.Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> GetList(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public int GetCount(ISpecification<TEntity> spec)
        {
            throw new NotImplementedException();
        }


        public IList<TEntity> GetList(ShardParams shardParams, IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}
