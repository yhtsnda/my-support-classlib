using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;

using Projects.Tool;
using Projects.Tool.Shards;
using Projects.Framework;

namespace Projects.Framework.NHibernateAccess
{
    public class NHibernateShardSessionFactory : IShardSessionFactory
    {
        SessionManager manager;
        ISpecificationProvider specificationProvider;
        object syscRoot = new object();

        public NHibernateShardSessionFactory()
        {
            specificationProvider = new NHibernateSpecificationProvider();
            manager = new SessionManager();
        }

        public ISpecificationProvider SpecificationProvider
        {
            get { return specificationProvider; }
        }

        public IShardSession<TEntity> OpenSession<TEntity>(ShardParams shardParams)
        {
            var strategy = RepositoryFramework.GetShardStrategy(typeof(TEntity));
            if (strategy == null)
                throw new ArgumentNullException("strategy", String.Format("无法找到类型 {0} 对应的分区策略信息。", typeof(TEntity).FullName));

            var shardId = strategy.GetShardId(shardParams);
            var partitionId = strategy.GetPartitionId(shardParams);

            var session = manager.OpenSession(shardId, partitionId);
            return new NHibernateShardSession<TEntity>(session);
        }
    }
}
