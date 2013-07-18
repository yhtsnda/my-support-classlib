using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Framework;

namespace Projects.UserCenter
{
    /// <summary>
    /// 接入用户中心的应用客户端
    /// </summary>
    public class ClientApp : IValidatable
    {
        /// <summary>
        /// 接入用户中心的应用客户端
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        public ClientApp(int code, string title, string description)
        {
            this.Code = code;
            this.Title = title;
            this.Description = description;
            this.CreateTime = DateTime.Now;
            this.Status = ClientAppStatus.Enabled;
            this.SecretCode = Guid.NewGuid().ToString("N").ToLower();
        }

        /// <summary>
        /// 应用代码(对应OAuth的ClientId)
        /// </summary>
        public virtual int Code { get; protected set; }

        /// <summary>
        /// 应用标题
        /// </summary>
        public virtual string Title { get; protected set; }

        /// <summary>
        /// 应用描述
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }

        /// <summary>
        /// 应用状态
        /// </summary>
        public virtual ClientAppStatus Status { get; protected set; }

        /// <summary>
        /// 应用秘钥(接入OAuth)
        /// </summary>
        public virtual string SecretCode { get; protected set; }
        
        /// <summary>
        /// 开关应用
        /// </summary>
        public virtual void SwitchClientApp(ClientAppStatus status)
        {
            if(this.Status != status)
                this.Status = status;
        }

        /// <summary>
        /// 变更秘钥
        /// </summary>
        public virtual void ChangeSecretCode()
        {
            this.SecretCode = Guid.NewGuid().ToString("N").ToLower();
        }

        void IValidatable.Validate()
        {
            if (Code < 1000 || Code > 9999)
                throw new ArgumentException("应用代码必须介于1000到9999之间");
            if (String.IsNullOrEmpty(Title))
                throw new ArgumentException("标题不能为空");
            if (Title.Length > 50)
                throw new ArgumentException("应用标题必须小于50个字符");
        }
    }
}
