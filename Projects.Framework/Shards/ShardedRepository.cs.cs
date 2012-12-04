using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework.Shards;

namespace Projects.Framework
{
    public abstract class AbstractShardRepository : IShardStrategy
    {
        public abstract void Init(IDictionary<string, string> attributes);

        public abstract ShardId GetShardId(ShardParams shardParams);

        public abstract PartitionId GetPartitionId(ShardParams shardParams);
    }
}
