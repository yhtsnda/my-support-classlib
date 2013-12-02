using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class UserInner : IValidatable
    {
        public UserInner()
        {
            RegTime = NetworkTime.Now;
        }

        /// <summary>
        /// 用户ID
        /// </summary>		
        public virtual long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码（编码后的）
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime RegTime { get; set; }

        /// <summary>
        /// 注册平台
        /// </summary>
        public virtual long RegPlatCode { get; set; }

        /// <summary>
        /// 升级状态
        /// </summary>
        public virtual UpgradeStatus UpgradeStatus { get; set; }

        public virtual bool ValidPassword(string password)
        {
            return !String.IsNullOrEmpty(password) && password.Equals(Password, StringComparison.CurrentCultureIgnoreCase);
        }

        void IValidatable.Validate()
        {
            Arguments.NotNullOrWhiteSpace(UserName, "UserName");
        }
    }
}
