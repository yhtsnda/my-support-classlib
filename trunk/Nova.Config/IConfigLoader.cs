using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    internal interface IConfigLoader
    {
        TConfig LoadSingle<TConfig>(string key);
        IDictionary<string, TConfig> LoadMutilPair<TConfig>();
        IList<TConfig> LoadMutilList<TConfig>();
    }
}
