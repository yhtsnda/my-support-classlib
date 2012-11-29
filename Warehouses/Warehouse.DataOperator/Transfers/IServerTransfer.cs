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
    /// 服务器间传输数据的接口定义,根据存储策略进行数据库赋值,
    /// 数据表创建及数据转移操作
    /// </summary>
    public interface IServerTransfer
    {
        /// <summary>
        /// 复制数据库结构,将源库中的所有数据表都复制到目标库中
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="source">需要复制的源数据库</param>
        /// <param name="target">需要复制到的目标数据库</param>
        ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target);

        /// <summary>
        /// 复制数据库结构,只复制指定的表
        /// 方法只复制数据数据库的结构,而不会拷贝数据
        /// </summary>
        /// <param name="tables">需要克隆的数据表</param>
        ResultKey Clone(StorageNodeConfigure source, StorageNodeConfigure target, IEnumerable tables);

        /// <summary>
        /// 根据数据存储策略创建数据表
        /// </summary>
        /// <param name="policy">数据的分布策略</param>
        ResultKey Generate(IPolicy policy);

        /// <summary>
        /// 传输数据表中的数据
        /// </summary>
        /// <param name="source">传输数据的源数据库</param>
        /// <param name="target">传输数据的目标数据库</param>
        /// <param name="policy">传输数据的策略</param>
        ResultKey Transmission(string source, string target, IPolicy policy);
    }
}
