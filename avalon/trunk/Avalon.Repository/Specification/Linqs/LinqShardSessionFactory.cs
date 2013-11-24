using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework
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

        public object GetConnectionManager()
        {
            throw new NotImplementedException();
        }


        public IShardSession OpenSession(Type entityType, ShardParams shardParams)
        {
            var type = typeof(LinqShardSession<>).MakeGenericType(entityType);
            return (IShardSession)FastActivator.Create(type);
        }
    }
}
