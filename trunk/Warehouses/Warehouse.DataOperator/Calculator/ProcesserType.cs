using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// 处理器类型
    /// </summary>
    public enum ProcesserType
    {
        /// <summary>
        /// 本地线程处理
        /// </summary>
        LocalThread,
        /// <summary>
        /// 远程服务处理
        /// </summary>
        RemoteService,
    }
}
