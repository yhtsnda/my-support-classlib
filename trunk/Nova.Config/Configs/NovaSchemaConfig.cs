using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public class NovaSchemaConfig
    {
        internal Dictionary<string, DataSourceConfig> DataSources { get; set; }

        internal Dictionary<string, DataNodeConfig> DataNodes { get; set; }

        internal Dictionary<string, SchemaConfig> Schemas { get; set; }

        public DataSourceConfig GetDataSource(string key)
        {
            if (this.DataSources.ContainsKey(key))
                return this.DataSources[key];
            return null;
        }

        public DataNodeConfig GetDataNode(string key)
        {
            if (this.DataNodes.ContainsKey(key))
                return this.DataNodes[key];
            return null;
        }

        public SchemaConfig GetSchema(string key)
        {
            if (this.Schemas.ContainsKey(key))
                return this.Schemas[key];
            return null;
        }
    }
}
