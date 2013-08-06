using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public class TableRuleConfig
    {
        public TableRuleConfig(string name, IList<RuleConfig> rules)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name is null");
            if (rules == null || rules.Count == 0)
                throw new ArgumentNullException("rule is null");

            this.Name = name;
            this.Rules = rules;
        }

        public string Name { get; protected set; }
        public IList<RuleConfig> Rules { get; protected set; }

    }
}
