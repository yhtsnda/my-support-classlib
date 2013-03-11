using Projects.Tool.Shards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    public class ShardConfig
    {
        Type entityType;

        internal ShardConfig(Type entityType)
        {
            this.entityType = entityType;
        }

        public ShardConfig ShardStrategy<TShardStrategy>(IDictionary<string, string> attributes = null) where TShardStrategy : IShardStrategy
        {
            RepositoryFramework.RegisterShardStragety(entityType, typeof(TShardStrategy), attributes);
            return this;
        }

        public ShardConfig ShardStrategy<TShardStrategy>(object attributes) where TShardStrategy : IShardStrategy
        {
            var pa = PropertyAccessorFactory.GetPropertyAccess(attributes.GetType());
            var d = pa.ToDictionary(attributes);
            var sd = new Dictionary<string, string>();
            foreach (DictionaryEntry entry in d)
            {
                sd.Add(entry.Key.ToString(), entry.Value.ToString());
            }
            return ShardStrategy<TShardStrategy>(sd);
        }

        public ShardConfig ShardSessionFactory<TShardSessionFactory>() where TShardSessionFactory : IShardSessionFactory
        {
            RepositoryFramework.RegisterShardSessionFactory(entityType, typeof(TShardSessionFactory));
            return this;
        }
    }

    public static class ShardConfiguationExtend
    {
        public static ShardConfig ShardConfig<TEntity>(this ClassDefine<TEntity> define)
        {
            return new ShardConfig(typeof(TEntity));
        }

    }
}
