using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    public abstract class AbstractShardStrategy : IShardStrategy
    {
        public abstract void Init(IDictionary<string, string> attributes);

        public abstract ShardId GetShardId(ShardParams shardParams);

        public abstract PartitionId GetPartitionId(ShardParams shardParams);
    }
}
