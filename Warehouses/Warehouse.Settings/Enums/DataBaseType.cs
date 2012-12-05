using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.Settings
{
    /// <summary>
    /// 存储介质
    /// </summary>
    public enum StorageMediaType
    {
        /// <summary>
        /// 文件方式
        /// </summary>
        File,

        /// <summary>
        /// SQL Server数据库
        /// </summary>
        SQLServer,

        /// <summary>
        /// MySQL数据库
        /// </summary>
        MySQL,
    }
}
