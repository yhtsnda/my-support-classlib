using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MongoDB.Driver;

using Projects.Tool;
using Projects.Tool.Reflection;
using Projects.Framework;
using Projects.Framework.Shards;

namespace Projects.Accesses.MongoAccess
{
    internal class MongoManager
    {
        Dictionary<ShardId, MongoDatabase> databases = new Dictionary<ShardId, MongoDatabase>();
        object syscRoot = new object();

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

            var database = GetDatabase(shardId);
            if (partitionId == null)
            {
                var metadata = RepositoryFramework.GetDefineMetadata(typeof(TEntity));
                if (metadata != null && !String.IsNullOrEmpty(metadata.Table))
                    return database.GetCollection<TEntity>(metadata.Table);

                return database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
            return database.GetCollection<TEntity>(partitionId.RealTableName);
        }

        MongoDatabase GetDatabase(ShardId shardId)
        {
            MongoDatabase database = databases.TryGetValue(shardId);
            if (database == null)
            {
                lock (syscRoot)
                {
                    database = databases.TryGetValue(shardId);
                    if (database == null)
                    {
                        database = OpenDatabase(shardId);
                        databases.Add(shardId, database);
                    }
                }
            }
            return database;
        }

        MongoDatabase OpenDatabase(ShardId shardId)
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            var shardNode = shardNodes.FirstOrDefault(o => o.Attributes.TryGetValue("id") == shardId.Id);
            if (shardNode == null)
                throw new MissConfigurationException(ToolSection.Instance.RootNode, "shard/shardIds/shardId");

            var connectionName = shardNode.Attributes.TryGetValue("connectionName");
            if (String.IsNullOrEmpty(connectionName))
                throw new ArgumentNullException("必须为分区 " + shardId.Id + " 指定数据源链接的名称路径 shard/shardIds/shardId/connectionName ");

            return MongoDatabase.Create(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
        }
    }
}
