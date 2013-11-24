using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalon.MongoAccess
{
    public class MongoServerDetector : AbstractServerDetector
    {
        static List<ShardId> shards;

        public override string Name
        {
            get { return "mongo"; }
        }

        static MongoServerDetector()
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            Dictionary<string, ShardId> shardsDic = shardNodes.ToDictionary(o => o.Attributes.TryGetValue("connectionName").ToLower(), o => new ShardId(o.Attributes.TryGetValue("id")));

            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                if (!IsMongoConnectionString(cs.ConnectionString))
                    shardsDic.Remove(cs.Name.ToLower());
            }
            shards = shardsDic.Values.ToList();
        }

        protected override string OnDetect()
        {
            var manager = (MongoManager)RepositoryFramework.GetSessionFactoryByFactoryType<MongoShardSessionFactory>().GetConnectionManager();
            List<string> outputs = new List<string>();
            Stopwatch sw = new Stopwatch();
            foreach (var shard in shards)
            {
                sw.Restart();
                var database = manager.GetConnection(shard);
                var value = database.Eval(new MongoDB.Bson.BsonJavaScript("new Date();"));
                sw.Stop();
                outputs.Add(String.Format("    {0} {1}", shard.Id, sw.ElapsedMilliseconds));
            }
            return String.Join("\r\n", outputs);
        }

        static bool IsMongoConnectionString(string conn)
        {
            return conn.IndexOf("mongo", StringComparison.CurrentCultureIgnoreCase) > -1;
        }
    }
}
