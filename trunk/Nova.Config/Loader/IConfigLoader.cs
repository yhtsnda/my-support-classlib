using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    internal interface IConfigLoader
    {
        NovaSchemaConfig GetNovaSchemaConfig();

        NovaRuleConfig GetNovaRuleConfig();
    }
}
