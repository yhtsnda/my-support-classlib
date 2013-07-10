using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Projects.Framework.Utils;
using Projects.Tool;
using Projects.Tool.Shards;
using NHibernate;

namespace Projects.Framework.NHibernateAccess
{
    public class NHibernateDatabaseServerDetector : AbstractServerDetector
    {
        static DateTime initTime;
        static List<ShardId> mysqlShards;

        static NHibernateDatabaseServerDetector()
        {
            var shardNodes = ToolSection.Instance.TryGetNodes("shard/shardIds/shardId");
            Dictionary<string, ShardId> shards = shardNodes.ToDictionary(o => o.Attributes.TryGetValue("connectionName").ToLower(), o => new ShardId(o.Attributes.TryGetValue("id")));

            foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
            {
                if (!IsMysqlConnectionString(cs.ConnectionString))
                    shards.Remove(cs.Name.ToLower());
            }
            mysqlShards = shards.Values.ToList();
            initTime = DateTime.Now;
        }

        static bool IsMysqlConnectionString(string conn)
        {
            return conn.IndexOf("Database", StringComparison.CurrentCultureIgnoreCase) > -1;
        }

        protected override void OnDetect()
        {
            var factory = (NHibernateShardSessionFactory)RepositoryFramework.GetSessionFactoryByFactoryType(typeof(NHibernateShardSessionFactory));
            foreach (var shard in mysqlShards)
            {
                var session = factory.SessionManager.OpenSession(shard, null);
                var now = session.CreateSQLQuery("SELECT NOW() d").AddScalar("d", NHibernateUtil.DateTime).UniqueResult<DateTime>();
            }
        }
    }
}
