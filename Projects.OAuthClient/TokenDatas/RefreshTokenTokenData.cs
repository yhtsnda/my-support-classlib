using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 更新凭证数据类
    /// </summary>
    public class RefreshTokenData : SimpleTokenData
    {
        /// <summary>
        /// 带更新凭据的凭据类
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="refreshToken">更新凭据</param>
        public RefreshTokenData(int clientId, string clientSecret, string refreshToken)
            : base(clientId, clientSecret)
        {
            this.RefreshToken = refreshToken;
        }

        public string RefreshToken { get; set; }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.REFRESH_TOKEN, this.RefreshToken);
            data.Add(Protocal.GRANT_TYPE, Protocal.REFRESH_TOKEN);

            base.UpdateDatas(data);
        }
    }
}
