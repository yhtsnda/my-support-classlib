using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    /// <summary>
    /// 默认使用param2的值
    /// </summary>
    public class WeekShardStrategy : AbstractShardStrategy
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
            param2 = attributes.TryGetValue("param2") == "true" || !attributes.ContainsKey("param2");//兼容之前的配置 TODO:hhb调整后需要去掉
        }

        public override PartitionId GetPartitionId(ShardParams shardParams)
        {
            var v = param2 ? shardParams.Param2 : shardParams.Param1;

            DateTime nowtime = DateTimeExtend.FromUnixTime(v);
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
