using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// 授权状态
    /// </summary>
    public enum ClientAuthorizeStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 1
    }
}
