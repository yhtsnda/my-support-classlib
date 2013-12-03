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
        public virtual int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 密码（编码后的）
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual string Mobile { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public virtual string IdCard { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public virtual string RegIp { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime RegTime { get; set; }
        /// <summary>
        /// 来源应用
        /// </summary>
        public virtual int SourceApp { get; set; }
        PlatType platType = PlatType.CommonWeb;
        /// <summary>
        /// 来源平台
        /// </summary>
        public virtual PlatType FromPlat { get { return platType; } set { platType = value; } }
        /// <summary>
        /// 用户状态状态
        /// </summary>
        public virtual UserStatus UpgradeStatus { get; set; }

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
