using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Config
{
    /// <summary>
    /// 架构类型
    /// </summary>
    public enum SchemaType
    {
        /// <summary>
        /// 单节点架构
        /// </summary>
        Single = 1,

        /// <summary>
        /// 共享节点架构
        /// </summary>
        Shard = 2
    }

    /// <summary>
    /// 架构配置
    /// </summary>
    public class SchemaConfig
    {
        private List<string> allDataNodes;

        public SchemaConfig(string name, SchemaType type, string defaultDataNodes,
            bool keepSchema, IDictionary<string, TableConfig> tables)
        {
            this.Name = name;
            this.Type = type;
            this.DefaultDataNode = DefaultDataNode;
            this.Tables = tables;
            this.KeepSqlSchema = keepSchema;
            SetAllDataNode();
        }

        /// <summary>
        /// 架构名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 架构类型
        /// </summary>
        public SchemaType Type { get; protected set; }

        /// <summary>
        /// 架构中默认使用的DataNode
        /// </summary>
        public string DefaultDataNode { get; protected set; }

        /// <summary>
        /// 架构中例外的Table配置
        /// </summary>
        public IDictionary<string, TableConfig> Tables { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool KeepSqlSchema { get; protected set; }

        /// <summary>
        /// 所有数据节点
        /// </summary>
        public List<string> AllDataNode { get; }

        private void SetAllDataNode()
        {
            if (!String.IsNullOrEmpty(this.DefaultDataNode))
                allDataNodes.Add(DefaultDataNode);
            if (this.Type == SchemaType.Shard)
            {
                foreach (var tc in this.Tables.Values)
                    allDataNodes.AddRange(tc.DataNodes);
            }
        }
    }
}
