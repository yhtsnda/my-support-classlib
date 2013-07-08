using Projects.Tool;
using Projects.Tool.Shards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework.Shards
{
    public class ModShardStrategy : AbstractShardStrategy
    {
        ShardId shardId;
        int mod;
        string table;
        string format;
        bool param2 = false;

        public int Mod
        {
            get { return mod; }
            set { mod = value; }
        }

        public override ShardId GetShardId(ShardParams shardParams)
        {
            return shardId;
        }

        public override PartitionId GetPartitionId(ShardParams shardParams)
        {
            var v = param2 ? shardParams.Param2 : shardParams.Param1;
            return new PartitionId(table, String.Format(format, v % mod));
        }

        public override void Init(IDictionary<string, string> attributes)
        {
            if (!attributes.ContainsKey("shard"))
                throw new ArgumentNullException("shard");

            if (!attributes.ContainsKey("mod"))
                throw new ArgumentNullException("mod");

            if (!attributes.ContainsKey("table"))
                throw new ArgumentNullException("table");

            if (!attributes.ContainsKey("format"))
                throw new ArgumentNullException("format");

            shardId = new ShardId(attributes["shard"]);
            mod = Int32.Parse(attributes["mod"]);
            table = attributes["table"];
            format = attributes["format"];
            param2 = attributes.TryGetValue("param2") == "true";
        }
    }
}
