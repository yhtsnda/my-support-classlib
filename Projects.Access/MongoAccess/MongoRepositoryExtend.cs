using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;
using Projects.Framework.Shards;

namespace Projects.Accesses.MongoAccess
{
    public static class MongoRepositoryExtend
    {
        public static MongoSession GetMongoSession<TEntity>(this AbstractRepository<TEntity> repository,
            ShardParams shardParams) where TEntity : class
        {
            var nr = (AbstractRepository<TEntity>)repository;
            return ((MongoShardSession<TEntity>)nr.OpenSession(shardParams)).InnerSession;
        }
    }
}
