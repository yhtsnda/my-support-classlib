using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Configure
{
    /// <summary>
    /// 数据分布类型
    /// </summary>
    public enum DataDistributeType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 单数据库单表存储
        /// </summary>
        SingleDatabaseSingleTable = 1,
        /// <summary>
        /// 单数据库多表存储
        /// </summary>
        SingleDatabaseMultiTable = 2,
        /// <summary>
        /// 多数据库单表存储(即数据表在每个数据库中只存在一个,且名称相同)
        /// </summary>
        MultiDatabaseSingleTable = 3,
        /// <summary>
        /// 多数据库多表存储
        /// </summary>
        MultiDatabaseMultiTable = 4,
    }
}
