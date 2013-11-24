using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    public class MonthShardStrategy : AbstractShardStrategy
    {
        string shard;
        string table;
        string format;
        bool debug = false;
        bool param2 = false;

        public override void Init(IDictionary<string, string> attributes)
        {
            shard = attributes.TryGetValue("shard");
            table = attributes.TryGetValue("table");
            format = attributes.TryGetValue("format");

            if (String.IsNullOrEmpty(shard))
                throw new ArgumentException("需要配置节属性 shard");
            if (String.IsNullOrEmpty(table))
                throw new ArgumentException("需要配置节属性 table");
            if (String.IsNullOrEmpty(format))
                throw new ArgumentException("需要配置节属性 format");

            debug = attributes.TryGetValue("debug") == "true";
            param2 = attributes.TryGetValue("param2") == "true";
        }

        public override ShardId GetShardId(ShardParams shardParams)
        {
            return new ShardId(shard);
        }

        public override PartitionId GetPartitionId(ShardParams shardParams)
        {
            var v = param2 ? shardParams.Param2 : shardParams.Param1;
            DateTime nowtime = DateTimeExtend.FromUnixTime(v);
            string tid;
            if (!debug)
            {
                tid = nowtime.Year.ToString() + nowtime.Month.ToString().PadLeft(2, '0');
            }
            else
            {
                tid = "00";
            }
            return new PartitionId(table, String.Format(format, tid));
        }
    }
}
