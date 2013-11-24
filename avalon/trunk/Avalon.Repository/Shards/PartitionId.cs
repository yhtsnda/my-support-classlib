using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Shards
{
    /// <summary>
    /// 分表的标识
    /// </summary>
    public class PartitionId
    {
        public PartitionId()
        { }

        public PartitionId(string table, string realTable)
        {
            TableName = table;
            RealTableName = realTable;
        }

        /// <summary>
        /// 真实的表名
        /// </summary>
        public string RealTableName { get; set; }

        /// <summary>
        /// 逻辑表名
        /// </summary>
        public string TableName { get; set; }

        public override int GetHashCode()
        {
            return TableName.GetHashCode() ^ RealTableName.GetHashCode();
        }
    }
}
