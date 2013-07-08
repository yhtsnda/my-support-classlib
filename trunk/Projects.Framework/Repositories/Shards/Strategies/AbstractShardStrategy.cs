using Projects.Tool;
using Projects.Tool.Shards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
    public abstract class AbstractShardStrategy : IShardStrategy
    {
        public abstract void Init(IDictionary<string, string> attributes);

        public abstract ShardId GetShardId(ShardParams shardParams);

        public abstract PartitionId GetPartitionId(ShardParams shardParams);
    }
}
