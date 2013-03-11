using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Projects.Tool;
using Projects.Tool.Reflection;
using Projects.Tool.Shards;
using Projects.Framework;

namespace Projects.Framework.MongoAccess
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
    }
}
