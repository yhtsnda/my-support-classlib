using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 不可用
        /// </summary>
        Disabled = 1,
        /// <summary>
        /// 被删除
        /// </summary>
        Deleted = 2
    }
}
