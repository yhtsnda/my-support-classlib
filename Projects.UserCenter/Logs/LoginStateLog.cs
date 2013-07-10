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
        public LoginStateLog(int userId)
        {
            this.UserId = userId;
            this.LastLogin = DateTime.Now;
            this.LoginTimes = 1;
        }

        public virtual int UserId { get; set; }

        public virtual DateTime LastLogin { get; set; }

        public virtual int LoginTimes { get; set; }

        public virtual void ChangeLoginState()
        {
            this.LoginTimes = DateTime.Now;
            this.LoginTimes += 1;
        }
    }
}
