using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Framework
{
    /// <summary>
    /// 操作结果键
    /// </summary>
    public enum ResultKey
    {
        /// <summary>
        /// 
        /// </summary>
        NotSet = 1,//默认从1开始，免得第一个总被默认 2011-05-05

        /// <summary>
        /// 操作结果为成功
        /// </summary>
        OK,

        /// <summary>
        /// 操作结果为失败
        /// </summary>
        Failure,

        /// <summary>
        /// 存在
        /// </summary>
        Exists,

        /// <summary>
        /// 不存在
        /// </summary>
        NotExists
    }
}
