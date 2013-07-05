using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 应用状态状态
    /// </summary>
    public enum ClientAppStatus
    {
        /// <summary>
        /// 启用状态,可注册,登录(包括第三方)
        /// </summary>
        Enabled = 0,
        
        /// <summary>
        /// 限制状态,可登录(包括第三方)不可注册
        /// </summary>
        Limited = 1,

        /// <summary>
        /// 关闭状态,不可注册,不可登录
        /// </summary>
        Disabled = 2
    }
}
