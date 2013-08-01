using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public abstract class AbstractConfigLoader : IConfigLoader
    {
        public IDictionary<string, IRuleAlgorithm> GetRuleFunction()
        {
            throw new NotImplementedException();
        }

        public IList<RuleConfig> GetRuleConfigs()
        {
            throw new NotImplementedException();
        }

        public SchemaConfig GetSchemaConfig(string schema)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, SchemaConfig> GetSchemaConfigs()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, DataNodeConfig> GetDataNodes()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, DataSourceConfig> GetDateSources()
        {
            throw new NotImplementedException();
        }

        public SystemConfig GetSystemConfig()
        {
            throw new NotImplementedException();
        }

        public UserConfig GetUserConfig()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, UserConfig> GetUserConfigs()
        {
            throw new NotImplementedException();
        }

        public QuarantineConfig GetQuarantineConfig()
        {
            throw new NotImplementedException();
        }

        public ClusterConfig GetClusterConfig()
        {
            throw new NotImplementedException();
        }

        protected abstract TConfig IConfigLoader.LoadSingle<TConfig>(string key);
        protected abstract IDictionary<string, TConfig> IConfigLoader.LoadMutilPair<TConfig>();
        protected abstract IList<TConfig> IConfigLoader.LoadMutilList<TConfig>();
    }
}
