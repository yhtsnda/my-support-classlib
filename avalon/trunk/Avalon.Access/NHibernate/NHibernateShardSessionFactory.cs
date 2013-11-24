using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Utility;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.NHibernateAccess
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

        public ISessionFactory GetSessionFactory(ShardId shardId)
        {
            return manager.GetConnection(shardId);
        }

        public ISessionFactory GetSessionFactory(Type entityType)
        {
            return GetSessionFactory(entityType, ShardParams.Empty);
        }

        public ISessionFactory GetSessionFactory(Type entityType, ShardParams shardParams)
        {
            var strategy = RepositoryFramework.GetShardStrategy(entityType);
            if (strategy == null)
                throw new ArgumentNullException("strategy", String.Format("无法找到类型 {0} 对应的分区策略信息。", entityType.FullName));

            var shardId = strategy.GetShardId(shardParams);
            return GetSessionFactory(shardId);
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

        public IShardSession OpenSession(Type entityType, ShardParams shardParams)
        {
            var strategy = RepositoryFramework.GetShardStrategy(entityType);
            if (strategy == null)
                throw new ArgumentNullException("strategy", String.Format("无法找到类型 {0} 对应的分区策略信息。", entityType.FullName));

            var shardId = strategy.GetShardId(shardParams);
            var partitionId = strategy.GetPartitionId(shardParams);
            var session = manager.OpenSession(shardId, partitionId);

            var nhsession = (INHibernateShardSession)FastActivator.Create(typeof(NHibernateShardSession<>).MakeGenericType(entityType));
            nhsession.InnerSession = session;
            return nhsession;
        }

        public object GetConnectionManager()
        {
            return manager;
        }

    }
}
