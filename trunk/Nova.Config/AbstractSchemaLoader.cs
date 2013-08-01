using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public abstract class AbstractSchemaLoader : IConfigLoader
    {
        public IDictionary<string, TableRuleConfig> GetTableRules()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IRuleAlgorithm> GetFunctions()
        {
            throw new NotImplementedException();
        }

        public IDictionary<String, DataSourceConfig> GetDataSources()
        {
            throw new NotImplementedException();
        }

        public IDictionary<String, DataNodeConfig> GetDataNodes()
        {
            throw new NotImplementedException();
        }

        public IDictionary<String, SchemaConfig> GetSchemas()
        {
            throw new NotImplementedException();
        }

        public IList<RuleConfig> GetRuleConfigs()
        {
            throw new NotImplementedException();
        }

        protected abstract TConfig IConfigLoader.LoadSingle<TConfig>(string key);
        protected abstract IDictionary<string, TConfig> IConfigLoader.LoadMutilPair<TConfig>();
        protected abstract IList<TConfig> IConfigLoader.LoadMutilList<TConfig>();
    }
}
