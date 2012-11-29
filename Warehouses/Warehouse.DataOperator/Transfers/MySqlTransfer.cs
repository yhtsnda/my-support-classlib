using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Warehouse.Settings;
using Warehouse.Utility;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// MySQL数据传输操作类
    /// </summary>
    public class MySqlTransfer : IServerTransfer
    {
        /// <summary>
        /// 复制数据库结构,将源库中的所有数据表都复制到目标库中
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="source">需要复制的源数据库</param>
        /// <param name="target">需要复制到的目标数据库</param>
        public ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 复制数据库结构,只复制指定的表
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="source">需要复制的源数据库</param>
        /// <param name="target">需要复制到的目标数据库</param>
        /// <param name="tables">需要克隆的数据表</param>
        public ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target, IEnumerable tables)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据数据存储策略创建数据表
        /// </summary>
        public ResultKey Generate(IPolicy policy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 传输数据表中的数据
        /// </summary>
        public ResultKey Transmission(string source, string target, IPolicy policy)
        {
            throw new NotImplementedException();
        }
    }
}
