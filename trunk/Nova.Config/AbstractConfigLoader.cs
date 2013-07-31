using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public abstract class AbstractConfigLoader : IConfigLoader
    {

        public TConfig LoadSingle<TConfig>(string key)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, TConfig> LoadMutilPair<TConfig>()
        {
            throw new NotImplementedException();
        }

        public IList<TConfig> LoadMutilList<TConfig>()
        {
            throw new NotImplementedException();
        }
    }
}
