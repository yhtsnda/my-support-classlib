using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nova.Config
{
    public class DefaultConfigLoader : IConfigLoader
    {
        private List<DataSourceConfig> datasources;
        private List<DataNodeConfig> datanodes;
        private Dictionary<string, TableRuleConfig> tableRules;


        public NovaSchemaConfig GetNovaSchemaConfig()
        {
            throw new NotImplementedException();
        }

        public NovaRuleConfig GetNovaRuleConfig()
        {
            throw new NotImplementedException();
        }

        private void GetDateSources(ConfigNode node)
        {
            var sourceName = node.Attributes["name"];
            var sourceType = node.Attributes["type"];
            if (sourceType.ToLower() != "mysql")
                throw new NotSupportedException("nova not support " + sourceType);

            var userName = node.TryGetNode("UserName").InnerText;
            var password = node.TryGetNode("Password").InnerText;
            var sqlMode = node.TryGetNode("SqlMode").InnerText;

            var locations = node.TryGetNodes("Locations/Location");
            var index = 0;
            foreach (var item in locations)
            {
                datasources.Add(new DataSourceConfig()
                {
                    SourceName = String.Format("{0}[{1}]", sourceName, index),
                    SourceType = sourceType,
                    UserName = userName,
                    Password = password,
                    SqlMode = sqlMode,
                    Host = item.Attributes["host"],
                    Port = Convert.ToInt32(item.Attributes["port"]),
                    Database = item.Attributes["database"]
                });
                index++;
            }
        }

        private void GetDataNode(ConfigNode node)
        {
            if (this.datasources == null)
                throw new Exception("please load datasource first!");

            var name = node.Attributes["name"];
            var master = node.TryGetNode("MasterSourceRef").InnerText;
            if (!this.datasources.Any(o => o.SourceName == master))
                throw new ConfigurationException("master data source not exists!");
            var slave = node.TryGetNode("SlaveSourceRef").InnerText;
            if (!this.datasources.Any(o => o.SourceName == slave))
                throw new ConfigurationException("slave data source not exists!");

            int poolSize = 200;
            Int32.TryParse(node.TryGetNode("PoolSize").InnerText, out poolSize);
            var heartbeatSql = node.TryGetNode("HeartBeatSql").InnerText;

            datanodes.Add(new DataNodeConfig
            {
                NodeName = name,
                MasterSource = master,
                SlaveSource = slave,
                PoolSize = poolSize,
                HeartbeatSQL = heartbeatSql
            });
        }

        private void GetSchema(ConfigNode node)
        {
        }

        private List<TableConfig> GetTableConfigs(IEnumerable<ConfigNode> tableConfigs)
        {
            throw new NotImplementedException();
        }
    }
}
