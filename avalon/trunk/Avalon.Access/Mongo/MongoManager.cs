using MongoDB.Driver;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalon.Framework;
using Avalon.Profiler;
namespace Avalon.MongoAccess
{
    public class MongoManager : AbstractConnectionManager<MongoDatabase>
    {
        static MongoManager()
        {
            var baseType = typeof(MongoMap<>);

            foreach (var assembly in RepositoryFramework.RepositoryAssemblies)
            {
                var types = assembly.GetExportedTypes().Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == baseType);
                foreach (var type in types)
                {
                    FastActivator.Create(type);
                }
            }
        }

        public MongoCollection<TEntity> GetCollection<TEntity>(ShardParams shardParams)
        {
            var strategy = RepositoryFramework.GetShardStrategy(typeof(TEntity));
            if (strategy == null)
                throw new ArgumentNullException("strategy", String.Format("无法找到类型 {0} 对应的分区策略信息。", typeof(TEntity).FullName));

            var shardId = strategy.GetShardId(shardParams);
            var partitionId = strategy.GetPartitionId(shardParams);

            var database = GetConnection(shardId);
            if (partitionId == null)
            {
                var metadata = RepositoryFramework.GetDefineMetadata(typeof(TEntity));
                if (metadata != null && !String.IsNullOrEmpty(metadata.Table))
                    return database.GetCollection<TEntity>(metadata.Table);

                return database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
            return database.GetCollection<TEntity>(partitionId.RealTableName);
        }

        protected override MongoDatabase CreateConnection(string connStr)
        {
            var mongoUrl = MongoUrl.Create(connStr);
            using (ProfilerContext.Watch("open mongo database"))
            {
                MongoClient client = new MongoClient(mongoUrl);
                return client.GetServer().GetDatabase(mongoUrl.DatabaseName);
            }
        }
    }
}
