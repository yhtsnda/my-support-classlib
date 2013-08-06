using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public class DefaultConfigLoader : IConfigLoader
    {

        public NovaSchemaConfig GetNovaSchemaConfig()
        {
            throw new NotImplementedException();
        }

        public NovaRuleConfig GetNovaRuleConfig()
        {
            throw new NotImplementedException();
        }
    }
}
