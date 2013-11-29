using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// 服务端的 Access token 信息
    /// </summary>
    public class AccessGrant
    {
        const int ExpireHours = 24 * 7;
        const int RefreshExpireDays = 30;

        protected AccessGrant()
        {
        }

        public AccessGrant(int clientId, int clientCode = 0, long userId = 0)
        {
            ClientId = clientId;
            ClientCode = clientCode;
            UserId = userId;

            AccessToken = Guid.NewGuid().ToString("N");
            RefreshToken = Guid.NewGuid().ToString("N");
            CreateTime = NetworkTime.Now;
            ExpireTime = CreateTime.AddHours(ExpireHours);
            RefreshExpireTime = CreateTime.AddDays(RefreshExpireDays);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Access Token
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 可用的范围
        /// </summary>
        public virtual string Scope { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// Refresh token的过期时间
        /// </summary>
        public virtual DateTime RefreshExpireTime { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 端编号
        /// </summary>
        public virtual int ClientCode { get; set; }

        /// <summary>
        /// 用户标识（为0表示与用户无关）
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 授权的类型
        /// </summary>
        public virtual GrantType GrantType { get; set; }

        public virtual bool IsExpire()
        {
            return ExpireTime < NetworkTime.Now;
        }

        public virtual bool IsRefreshExpire()
        {
            return RefreshExpireTime < NetworkTime.Now;
        }

        public virtual AccessGrantModel ToModel()
        {
            return new AccessGrantModel
            {
                access_token = AccessToken,
                refresh_token = RefreshToken,
                client_id = ClientId,
                expire_in = (int)(ExpireTime - NetworkTime.Now).TotalSeconds,
                create_timestamp = (int)CreateTime.ToUnixTime(),
                scope = Scope,
                user_id = UserId
            };
        }
    }

    public class AccessGrantModel
    {
        /// <summary>
        /// APP_KEY
        /// </summary>
        public int client_id { get; set; }
        /// <summary>
        /// access token，过期时间为7天
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// refresh token，过期时间为1个月
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 距当前时间过期的秒数
        /// </summary>
        public int expire_in { get; set; }
        /// <summary>
        /// 创建该token的时间戳
        /// </summary>
        public int create_timestamp { get; set; }
        /// <summary>
        /// 许可的范围
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 用户的标识，如果为APP授权则该值为0
        /// </summary>
        public long user_id { get; set; }
    }
}
