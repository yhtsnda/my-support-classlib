using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    public class OAuthClientSetting
    {
        public OAuthClientSetting()
        {

        }

        public OAuthClientSetting(string openIdUrl, string authorizeUrl, string tokenUrl, string consumerKey, string callbackUrl, string consumerSecret, string scopr = "basic")
        {
            this.OpenIdUrl = openIdUrl;
            this.AuthorizeUrl = authorizeUrl;
            this.TokenUrl = tokenUrl;
            this.CallbackUrl = callbackUrl;
            this.ConsumerKey = consumerKey;
            this.Scope = scopr;
            this.ConsumerSecret = consumerSecret;
        }

        /// <summary>
        /// 请求用户授权Token的地址
        /// </summary>
        public string AuthorizeUrl { get; private set; }

        /// <summary>
        /// 获取授权过的Access Token的地址
        /// </summary>
        public string TokenUrl { get; private set; }

        /// <summary>
        /// 本站点的回调地址
        /// </summary>
        public string CallbackUrl { get; private set; }

        public string OpenIdUrl { get; private set; }

        /// <summary>
        /// 应用的标识
        /// </summary>
        public string ConsumerKey { get; private set; }

        /// <summary>
        /// 应用的密钥
        /// </summary>
        public string ConsumerSecret { get; private set; }

        /// <summary>
        /// 应用的范围
        /// </summary>
        public string Scope { get; private set; }
    }
}
