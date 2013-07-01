using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 登录情况日志
    /// </summary>
    public class LoginStateLog
    {
        public int UserId { get; set; }

        public DateTime LastLogin { get; set; }

        public int LoginTimes { get; set; }
    }
}
