using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    public class TableConfig
    {
        public TableConfig(string name, string dataNode, TableRuleConfig rule)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("table name is null");
            if (String.IsNullOrEmpty(dataNode))
                throw new ArgumentNullException("data node is null");

            this.TableName = name.ToUpper();
            var tmpDataNode = dataNode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpDataNode.Length == 0)
                throw new ArgumentNullException("table data node is null");
            this.DataNodes = tmpDataNode.ToList();
            this.Rule = rule;
            this.ColumnIndex = BuildColumnIndex(rule);
            this.HasRule = this.Rule != null;
        }

        public string TableName { get; protected set; }
        public List<string> DataNodes { get; protected set; }
        public List<string> ColumnIndex { get; protected set; }
        public bool HasRule { get; protected set; }
        public TableRuleConfig Rule { get; protected set; }

        private List<string> BuildColumnIndex(TableRuleConfig rule)
        {
            List<string> columnIndex = new List<string>();

            foreach (var ruleConfig in rule.Rules)
            {
                foreach (var col in ruleConfig.Columns)
                {
                    if(String.IsNullOrEmpty(col))
                        columnIndex.Add(col);
                }
            }
            return columnIndex;
        }
    }
}
