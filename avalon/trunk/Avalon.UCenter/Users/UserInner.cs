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
        protected UserInner()
        {
            RegTime = NetworkTime.Now;
        }

        public UserInner(string userName, string password, int sourceApp,
            PlatType platType = PlatType.CommonWeb,
            string email = "", string mobile = "", string idCard = "") : this()
        {
            this.UserName = userName;
            this.Password = password;
            this.SourceApp = sourceApp;
            this.FromPlat = platType;
            this.UserStatus = UCenter.UserStatus.Normal;
            this.RegIp = Avalon.Utility.IpAddress.GetIP();

            this.AllowLoginApps = new List<int>()
            {
                sourceApp
            };
        }

        /// <summary>
        /// 用户ID
        /// </summary>		
        public virtual int UserId { get; protected set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; protected set; }
        /// <summary>
        /// 密码（编码后的）
        /// </summary>
        public virtual string Password { get; protected set; }
        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; protected set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual string Mobile { get; protected set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public virtual string IdCard { get; protected set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public virtual string RegIp { get; protected set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime RegTime { get; protected set; }
        /// <summary>
        /// 来源应用
        /// </summary>
        public virtual int SourceApp { get; protected set; }
        PlatType platType = PlatType.CommonWeb;
        /// <summary>
        /// 来源平台
        /// </summary>
        public virtual PlatType FromPlat { get { return platType; } set { platType = value; } }
        /// <summary>
        /// 用户状态状态
        /// </summary>
        public virtual UserStatus UserStatus { get; set; }
        /// <summary>
        /// 允许用户进行登录的APP,默认包含其注册来源
        /// </summary>
        public virtual IList<int> AllowLoginApps { get; protected set; }

        public virtual bool ValidPassword(string password)
        {
            return !String.IsNullOrEmpty(password) && password.Equals(Password, StringComparison.CurrentCultureIgnoreCase);
        }

        void IValidatable.Validate()
        {
            Arguments.NotNullOrWhiteSpace(UserName, "UserName");

            var platService = Avalon.Framework.DependencyResolver.Resolve<PlatformService>();
            var plat = platService.GetPlatform(this.SourceApp);
            if (plat == null)
                throw new AvalonException("来源平台无效");

            if (plat.UsedVoucher.Contains(LoginVouchers.Email) &&
                String.IsNullOrWhiteSpace(this.Email))
                throw new ArgumentNullException("用户凭证中包含Email,则Email不能为空");
            if (plat.UsedVoucher.Contains(LoginVouchers.Mobile) &&
                String.IsNullOrWhiteSpace(this.Mobile))
                throw new ArgumentNullException("用户凭证中包含Mobile,则Mobile不能为空");
            if (plat.UsedVoucher.Contains(LoginVouchers.IdCard) &&
                String.IsNullOrWhiteSpace(this.IdCard))
                throw new ArgumentNullException("用户凭证中包含IdCard,则IdCard不能为空");
            
        }
    }
}
