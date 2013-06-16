using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 客户端授权状态
    /// </summary>
    public enum ClientAuthStatus
    {
        /// <summary>
        /// 正常,可用
        /// </summary>
        Enabled = 0, 

        /// <summary>
        /// 不可用
        /// </summary>
        Disabled = 1
    }
}
