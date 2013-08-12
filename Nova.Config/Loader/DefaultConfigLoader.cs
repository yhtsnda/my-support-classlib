using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Nova.Config
{
    public class DefaultConfigLoader : IConfigLoader
    {
        public Dictionary<string, IRuleAlgorithm> Functions { get; protected set; }
        public Dictionary<string, DataSourceConfig> Datasources { get; protected set; }
        public Dictionary<string, DataNodeConfig> Datanodes { get; protected set; }

        private static DefaultConfigLoader loader;

        public DefaultConfigLoader()
        {
            GetNovaSchemaConfig();
            GetNovaRuleConfig();
        }

        public static DefaultConfigLoader Instance
        {
            get
            {
                if (loader == null)
                    loader = new DefaultConfigLoader();
                return loader;
            }
        }

        public void GetNovaSchemaConfig()
        {
            throw new NotImplementedException();
        }

        public void GetNovaRuleConfig()
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
                Datasources.Add(String.Format("{0}[{1}]", sourceName, index), new DataSourceConfig()
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
            if (this.Datasources == null)
                throw new Exception("please load datasource first!");

            var name = node.Attributes["name"];
            var master = node.TryGetNode("MasterSourceRef").InnerText;
            if (!this.Datasources.ContainsKey(master))
                throw new ConfigurationException("master data source not exists!");
            var slave = node.TryGetNode("SlaveSourceRef").InnerText;
            if (!this.Datasources.ContainsKey(slave))
                throw new ConfigurationException("slave data source not exists!");

            int poolSize = 200;
            Int32.TryParse(node.TryGetNode("PoolSize").InnerText, out poolSize);
            var heartbeatSql = node.TryGetNode("HeartBeatSql").InnerText;

            this.Datanodes.Add(name, new DataNodeConfig
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
            if (this.Datanodes == null || this.Datanodes.Count == 0)
                throw new Exception("please load datanode first!");

            SchemaType sType = SchemaType.Single;
            //获取Schema的type
            if (!Enum.TryParse<SchemaType>(node.Attributes["type"], out sType))
                throw new Exception("schema type error, mast Single or Shard");

            if (sType == SchemaType.Single)
            {

            }
            else if (sType == SchemaType.Shard)
            {
            }
        }

        private void GetTableConfigs(IEnumerable<ConfigNode> tableConfigs)
        {
            throw new NotImplementedException();
        }

        private void GetFunctions(IEnumerable<ConfigNode> funcConfigs)
        {
            throw new NotImplementedException();
        }
    }
}
