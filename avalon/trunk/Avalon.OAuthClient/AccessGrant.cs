using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 服务端的 Access token 信息
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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 用户标识（为0表示与用户无关）
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 可用的范围
        /// </summary>
        public virtual string Scope { get; set; }

        public virtual bool IsExpire()
        {
            return ExpireTime < NetworkTime.Now;
        }

        internal class InnerAccessGrant
        {
            public string access_token;

            public string refresh_token;

            public int expire_in;

            public int create_timestamp;

            public int client_id;

            public long user_id;

            public string scope;

            public AccessGrant Convert()
            {
                return new AccessGrant()
                {
                    AccessToken = access_token,
                    ClientId = client_id,
                    ExpireTime = NetworkTime.Now.AddSeconds(expire_in),
                    RefreshToken = refresh_token,
                    CreateTime = DateTimeExtend.FromUnixTime(create_timestamp),
                    Scope = scope,
                    UserId = user_id
                };
            }
        }
    }
}
