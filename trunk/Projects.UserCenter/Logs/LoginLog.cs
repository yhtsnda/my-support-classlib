using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool;

namespace Projects.UserCenter
{
    /// <summary>
    /// 登录日志类
    /// </summary>
    public class LoginLog
    {
        public LoginLog(int userId, string userName, long regplatCode)
        {
            this.UserId = userId;
            this.UserName = userName;
            SpiltRegPlatCode(regplatCode);
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserId { get; protected set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; protected set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public virtual int ClientAppId { get; protected set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public virtual int TerminalType { get; protected set; }

        /// <summary>
        /// 账号来源码
        /// </summary>
        public virtual int SourceCode { get; protected set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }

        private void SpiltRegPlatCode(long code)
        {
            Arguments.That(code.ToString().Length == 12, "code", "regplat code must 12 - bit integer");

            this.ClientAppId = Convert.ToInt32(code.ToString().Substring(0, 4));
            this.TerminalType = Convert.ToInt32(code.ToString().Substring(4, 4));
            this.SourceCode = Convert.ToInt32(code.ToString().Substring(8, 4));
        }
    }
}
