using Avalon.Framework;
using Avalon.Framework.Shards;
using Avalon.Utility;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class NHibernateDatabaseServerDetector : AbstractServerDetector
    {
        static List<ShardId> shards;

        public override string Name
        {
            get { return "mysql"; }
        }

        static NHibernateDatabaseServerDetector()
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            Dictionary<string, ShardId> shardsDic = shardNodes.ToDictionary(o => o.Attributes.TryGetValue("connectionName").ToLower(), o => new ShardId(o.Attributes.TryGetValue("id")));

            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                if (!IsMysqlConnectionString(cs.ConnectionString))
                    shardsDic.Remove(cs.Name.ToLower());
            }
            shards = shardsDic.Values.ToList();
        }

        static bool IsMysqlConnectionString(string conn)
        {
            return conn.IndexOf("Database", StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        protected override string OnDetect()
        {
            var manager = (SessionManager)RepositoryFramework.GetSessionFactoryByFactoryType<NHibernateShardSessionFactory>().GetConnectionManager();
            List<string> outputs = new List<string>();
            Stopwatch sw = new Stopwatch();
            foreach (var shard in shards)
            {
                sw.Restart();
                var session = manager.OpenSession(shard, null);
                var now = session.CreateSQLQuery("SELECT NOW() d").AddScalar("d", NHibernateUtil.DateTime).UniqueResult<DateTime>();
                sw.Stop();
                outputs.Add(String.Format("    {0} {1}", shard.Id, sw.ElapsedMilliseconds));
            }
            return String.Join("\r\n", outputs);
        }
    }
}
