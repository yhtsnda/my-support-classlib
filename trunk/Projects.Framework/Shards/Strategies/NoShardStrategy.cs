using Projects.Framework.Shards;
using Projects.Tool;
using Projects.Tool.Shards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
    public class NoShardStrategy : AbstractShardStrategy
    {
        ShardId shardId;

        public override ShardId GetShardId(ShardParams shardParams)
        {
            return shardId;
        }

        public override PartitionId GetPartitionId(ShardParams shardParams)
        {
            return null;
        }

        public override void Init(IDictionary<string, string> attributes)
        {
            shardId = new ShardId(attributes.TryGetValue("shard"));
        }
    }
}
