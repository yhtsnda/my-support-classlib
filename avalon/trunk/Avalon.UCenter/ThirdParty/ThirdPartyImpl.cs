using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    /// <summary>
    /// 第三方相关接口
    /// </summary>
    public class ThirdPartyService : IService
    {
        Dictionary<MappingType, OAuthClient> clients = new Dictionary<MappingType, OAuthClient>();

        public ThirdPartyService()
        {
            clients.Add(MappingType.SinaWeibo, new WeiboOAuthClient(new OAuthClientSetting("https://api.weibo.com/2/account/get_uid.json", "https://api.weibo.com/oauth2/authorize", "https://api.weibo.com/oauth2/access_token", "2378040874", UserCenterConfig.UserSiteUrl, "2f21b43e0de48817ad04e1bbec5841f7")));
            clients.Add(MappingType.Tencent, new TencentOAuthClient(new OAuthClientSetting("https://graph.qq.com/oauth2.0/me", "https://graph.qq.com/oauth2.0/authorize", "https://graph.qq.com/oauth2.0/authorize", "100282952", UserCenterConfig.UserSiteUrl, "91789f2ab8da1fd85ef5a09b322d60d4")));
        }

        OAuthClient GetClient(MappingType source)
        {
            OAuthClient client = clients.TryGetValue(source);
            if (client == null)
                throw new ArgumentException("不支持的类型。" + source.ToString());

            return client;
        }

        public string GetUserKey(string accessToken, MappingType source)
        {
            var client = GetClient(source);
            return client.GetUserKey(accessToken);
        }

        /// <summary>
        /// 获取授权的地址
        /// </summary>
        /// <param name="source"></param>
        /// <param name="regPlat"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public string GetAuthorizeUrl(MappingType source, long regPlat, string redirectUrl, string clientId)
        {
            var client = GetClient(source);
            return client.GetAuthorizeUrl(source, regPlat, redirectUrl, clientId);
        }

        /// <summary>
        /// 根据 code 换 access_token
        /// </summary>
        /// <param name="source"></param>
        /// <param name="authorizationCode"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        public string GetAccessToken(string authorizationCode, MappingType source, long regPlat, string redirectUrl,int clientId)
        {
            var client = GetClient(source);
            return client.GetAccessToken(authorizationCode, source, regPlat, redirectUrl, clientId);
        }
    }
}
