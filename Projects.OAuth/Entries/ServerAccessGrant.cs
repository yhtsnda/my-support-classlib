using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 服务端的Access Token 信息
    /// </summary>
    public class ServerAccessGrant
    {
        //默认的过期时间为一天
        private const int DEFAULT_EXPIRE_SECONDS = 3600 * 24;
        private const int DEFAULT_REFRESH_EXPIRE_SECONDS = 3600 * 24 * 30;

        public ServerAccessGrant(int clientId, int userId = 0)
        {
            this.ClientId = clientId;
            this.UserId = userId;

            this.AccessToken = Guid.NewGuid().ToString("N");
            this.RefreshToken = Guid.NewGuid().ToString("N");
            //重置创建时间
            this.CreateTime = DateTime.Now;
            this.ExpireTime = this.CreateTime.AddSeconds(DEFAULT_EXPIRE_SECONDS);
            this.RefreshExpireTime = this.CreateTime.AddSeconds(DEFAULT_REFRESH_EXPIRE_SECONDS);
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 验证凭证
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 票据可控制的范围
        /// </summary>
        public virtual string Scope { get; set; }

        /// <summary>
        /// 可刷新凭证
        /// </summary>
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// Refresh token的过期时间
        /// </summary>
        public virtual DateTime RefreshExpireTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 用户标示(0表示与用户无关)
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 授权的类型
        /// </summary>
        public virtual GrantType GrantType { get; set; }

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

        /// <summary>
        /// 判断刷新票据是否有效
        /// </summary>
        /// <returns>
        /// True = 刷新票据有效
        /// False = 刷新票据失效
        /// </returns>
        public virtual bool IsRefreshEffective()
        {
            return this.RefreshExpireTime >= DateTime.Now;
        }
    }
}
