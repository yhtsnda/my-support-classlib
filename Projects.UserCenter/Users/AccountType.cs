using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// 通用账号,所有接入用户中心的系统都可以使用
        /// </summary>
        Universal,

        /// <summary>
        /// 限制账号(默认),只能在注册来源平台上使用
        /// </summary>
        Limited
    }
}
