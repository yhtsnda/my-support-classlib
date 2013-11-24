using MongoDB.Driver;
using Avalon.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;
using Avalon.Framework.Shards;

namespace Avalon.MongoAccess
{
    internal interface IMongoShardSession : IShardSession
    {
        MongoSession InnerSession { get; set; }
        ShardParams ShardParams { get; set; }

    }
    internal class MongoShardSession<TEntity> : IShardSession<TEntity>, IMongoShardSession
    {
        MongoSession session;
        ShardParams shardParams;

        public MongoShardSession()
        {
        }

        public MongoShardSession(MongoSession session, ShardParams shardParams)
        {
            this.session = session;
            this.shardParams = shardParams;
        }

        public MongoSession InnerSession
        {
            get { return session; }
            set { session = value; }
        }

        public ShardParams ShardParams
        {
            get { return shardParams; }
            set { shardParams = value; }
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
