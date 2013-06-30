using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 服务端的AccessToken信息
    /// </summary>
    public class AccessGrant
    {
        /// <summary>
        /// Access Token
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 用户标识（为0表示与用户无关）
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 可用的范围
        /// </summary>
        public virtual string Scope { get; set; }

        /// <summary>
        /// 判断票据是否有效
        /// </summary>
        /// <returns>
        /// True = 票据有效
        /// False = 票据失效
        /// </returns>
        public virtual bool IsEffective()
        {
            return this.ExpireTime >= DateTime.Now;
        }
    }
}
