using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.UserCenter
{
    /// <summary>
    /// 注册日志类
    /// </summary>
    public class RegisterLog
    {
        /// <summary>
        /// 注册日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="regplatCode">注册码(12位)</param>
        public RegisterLog(int userId, string userName, long regplatCode)
        {

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
    }
}
