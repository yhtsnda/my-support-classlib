using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UCenter
{
    /// <summary>
    /// 定义可连接UCenter的平台特性
    /// </summary>
    public class Platform : IValidatable
    {
        protected Platform()
        {
            this.Code = Guid.NewGuid().ToString("N");
            this.CreateTime = NetworkTime.Now;
            this.Status = PlatformStatus.InUse;
        }

        public Platform(string name, List<LoginVouchers> vouchers, bool allowThirdParty)
            : this()
        {
            this.AppName = name;
            //至少保证能够通过用户名进行登录
            if (vouchers != null && vouchers.Count > 0)
                this.UsedVoucher = vouchers;
            else
                this.UsedVoucher = new List<LoginVouchers> { LoginVouchers.UserName };
            this.AllowThirdParty = allowThirdParty;
        }

        /// <summary>
        /// 应用ID(只读)
        /// </summary>
        public virtual int Id { get; protected set; }
        /// <summary>
        /// 应用代码(只读)
        /// </summary>
        public virtual string Code { get; protected set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public virtual string AppName { get; set; }
        /// <summary>
        /// 可用登陆凭证类型
        /// </summary>
        public virtual List<LoginVouchers> UsedVoucher { get; set; }
        /// <summary>
        /// 允许第三方账号登陆
        /// </summary>
        public virtual bool AllowThirdParty { get; set; }
        /// <summary>
        /// 平台创建时间(只读)
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }
        /// <summary>
        /// 平台状态
        /// </summary>
        public PlatformStatus Status { get; set; }

        void IValidatable.Validate()
        {
            if (String.IsNullOrEmpty(this.AppName))
                throw new AvalonException("App名称不能为空");
        }
    }
    /// <summary>
    /// 登陆平台凭证类型
    /// </summary>
    public enum LoginVouchers
    {
        UserName,
        Mobile,
        IdCard
    }
    /// <summary>
    /// 平台状态
    /// </summary>
    public enum PlatformStatus
    {
        /// <summary>
        /// 不可用
        /// </summary>
        NotAvailable,
        /// <summary>
        /// 使用中
        /// </summary>
        InUse,
        /// <summary>
        /// 测试状态
        /// </summary>
        OnTesting
    }
}
