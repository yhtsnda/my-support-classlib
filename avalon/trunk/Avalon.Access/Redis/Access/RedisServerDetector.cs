using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalon.Access
{
    public class RedisServerDetector : AbstractServerDetector
    {
        static List<ShardId> shards;

        public override string Name
        {
            get { return "redis"; }
        }

        static RedisServerDetector()
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            Dictionary<string, ShardId> shardsDic = shardNodes.ToDictionary(o => o.Attributes.TryGetValue("connectionName").ToLower(), o => new ShardId(o.Attributes.TryGetValue("id")));

            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                if (!IsRedisConnectionString(cs.ConnectionString))
                    shardsDic.Remove(cs.Name.ToLower());
            }
            shards = shardsDic.Values.ToList();
        }

        protected override string OnDetect()
        {
            var manager = RedisManager.Instance;
            List<string> outputs = new List<string>();
            Stopwatch sw = new Stopwatch();
            foreach (var shard in shards)
            {
                sw.Restart();
                using (var client = manager.CreateRedisClient(shard))
                {
                    var size = client.DbSize;
                }
                sw.Stop();
                outputs.Add(String.Format("    {0} {1}", shard.Id, sw.ElapsedMilliseconds));
            }
            return String.Join("\r\n", outputs);
        }

        static bool IsRedisConnectionString(string conn)
        {
            return conn.IndexOf("readwrite server", StringComparison.CurrentCultureIgnoreCase) > -1;
        }

    }
}
