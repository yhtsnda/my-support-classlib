using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using Projects.Tool;
using Projects.Tool.Shards;
using Projects.Framework;

namespace Projects.Framework.MongoAccess
{
    internal class MongoShardSession<TEntity> : IShardSession<TEntity>
    {
        MongoSession session;
        ShardParams shardParams;

        public MongoShardSession(MongoSession session, ShardParams shardParams)
        {
            this.session = session;
            this.shardParams = shardParams;
        }

        public MongoSession InnerSession
        {
            get { return session; }
        }

        public void Create(TEntity entity)
        {
            session.Create(shardParams, entity);
        }

        public void Update(TEntity entity)
        {
            session.Update(shardParams, entity);
        }

        public void Delete(TEntity entity)
        {
            session.Delete(shardParams, entity);
        }

        public void SessionEvict(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool SessionContains(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            return session.Get<TEntity>(shardParams, id);
        }

        public IList<TEntity> GetList(IEnumerable ids)
        {
            return session.GetList<TEntity>(shardParams, ids);
        }

        public TEntity FindOne(ISpecification<TEntity> spec)
        {
            return session.FindOne<TEntity>(spec);
        }

        public IList<TEntity> FindAll(ISpecification<TEntity> spec)
        {
            return session.FindAll<TEntity>(spec);
        }

        public PagingResult<TEntity> FindPaging(ISpecification<TEntity> spec)
        {
            return session.FindPaging<TEntity>(spec);
        }

        public int Count(ISpecification<TEntity> spec)
        {
            return session.Count<TEntity>(spec);
        }

        public void Dispose()
        {

        }

    }
}
