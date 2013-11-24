using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.MongoAccess
{
    public class MongoShardSessionFactory : IShardSessionFactory
    {
        ISpecificationProvider specificationProvider;
        MongoManager mongoManager;

        public MongoShardSessionFactory()
        {
            specificationProvider = new MongoSpecificationProvider();
            mongoManager = new MongoManager();
        }

        public ISpecificationProvider SpecificationProvider
        {
            get { return specificationProvider; }
        }

        public IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams)
        {
            return new MongoShardSession<TEntity>(new MongoSession(mongoManager), shardParams);
        }

        public object GetConnectionManager()
        {
            return mongoManager;
        }


        public IShardSession OpenSession(Type entityType, ShardParams shardParams)
        {
            var session = (IMongoShardSession)FastActivator.Create(typeof(MongoShardSession<>).MakeGenericType(entityType));
            session.InnerSession = new MongoSession(mongoManager);
            session.ShardParams = shardParams;
            return session;
        }
    }
}
