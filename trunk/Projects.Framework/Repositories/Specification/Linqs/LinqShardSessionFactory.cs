using Projects.Tool;
using Projects.Tool.Shards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class LinqShardSessionFactory : IShardSessionFactory
    {
        ISpecificationProvider specificationProvider;

        public LinqShardSessionFactory()
        {
            specificationProvider = new LinqSpecificationProvider();
        }

        public ISpecificationProvider SpecificationProvider
        {
            get { return specificationProvider; }
        }

        public IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams)
        {
            return new LinqShardSession<TEntity>();
        }
    }
}
