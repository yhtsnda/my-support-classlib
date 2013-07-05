using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Framework;

namespace Projects.UserCenter
{
    public class User : IValidatable
    {
        public User(string userName, string password, long regplatcode, 
            string nickName = null, string realName = null, string email = null, 
            AccountType type= AccountType.Limited)
        {
            this.UserName = userName;
            //当用户的密码长度小于30,认为没有加密过
            if (password.Length < 30)
                this.Password = UserCenterUtility.EncryptPassword(password).ToLower();
            else
                this.Password = password.ToLower();
            this.RegPlatCode = regplatcode;
            this.NickName = NickName;
            this.RealName = realName;
            this.Email = email;
            this.AccountType = type;
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
        /// 用户密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string RealName { get; set; }

        /// <summary>
        /// 电邮
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 注册平台码
        /// </summary>
        public virtual long RegPlatCode { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public virtual AccountType AccountType { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }

        /// <summary>
        /// 账户的状态
        /// </summary>
        public virtual UserStatus Status { get; protected set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual string ExtendField { get; protected set; }

        /// <summary>
        /// 验证密码正确性
        /// </summary>
        /// <param name="inputPassword">输入的密码</param>
        /// <returns>匹配情况</returns>
        public virtual bool ValidatePassword(string inputPassword)
        {
            return String.Equals(Password, inputPassword, 
                StringComparison.InvariantCultureIgnoreCase);
        }

        void IValidatable.Validate()
        {
            if (String.IsNullOrWhiteSpace(UserName))
                throw new ArgumentException("用户名不能为空");
            if (UserName.Length > 50)
                throw new ArgumentException("用户名长度不能超过50");

            if (RegPlatCode < 0 || RegPlatCode.ToString().Length != 12)
                throw new ArgumentException("无效的平台码");

            if (RealName.Length > 50)
                throw new ArgumentException("真实姓名长度不能超过50字符");
            if (NickName.Length > 50)
                throw new ArgumentException("昵称长度不能超过50字符");
            if (Email.Length > 50)
                throw new ArgumentException("电子邮件长度不能超过50字符");
        }
    }
}
