using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    /// <summary>
    /// 可映射到用户中心的APP
    /// </summary>
    public class MappingApp : IValidatable
    {
        public MappingApp(string name, string suffix)
        {
            this.AppName = name;
            this.AppSuffix = suffix;
            this.Status = PlatformStatus.InUse;
        }
        /// <summary>
        /// 映射应用的ID
        /// </summary>
        public virtual int Id { get; protected set; }
        /// <summary>
        /// 映射应用的名称
        /// </summary>
        public virtual string AppName { get; protected set; }
        /// <summary>
        /// 映射应用在注册用户中心账号时,使用的后缀
        /// </summary>
        public virtual string AppSuffix { get; protected set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }
        /// <summary>
        /// 平台状态
        /// </summary>
        public virtual PlatformStatus Status { get; protected set; }

        public virtual void SetMappAppStatus(PlatformStatus status)
        {
            this.Status = status;
            this.CreateTime = NetworkTime.Now;
        }

        void IValidatable.Validate()
        {
            if (String.IsNullOrWhiteSpace(this.AppName))
                throw new ArgumentNullException("AppName不能为空");
        }
    }
}
