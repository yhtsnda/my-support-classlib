using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;

namespace Projects.Framework.Shards
{
    public class WeekShardStrategy : AbstractShardStrategy
    {
        string shard;
        string table;
        string format;
        bool debug = false;

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
        }

        public override PartitionId GetPartitionId(ShardParams shardParams)
        {
            DateTime nowtime = DateTimeExtend.FromUnixTime(shardParams.Param2);
            string tid;
            if (!debug)
            {
                int week = DateTimeExtend.WeekOfDate(nowtime);
                tid = nowtime.Year.ToString() + week.ToString().PadLeft(2, '0');
            }
            else
            {
                tid = nowtime.Year.ToString() + "00";
            }
            return new PartitionId(table, String.Format(format, tid));
        }

        public override ShardId GetShardId(ShardParams shardParams)
        {
            return new ShardId(shard);
        }

    }
}
