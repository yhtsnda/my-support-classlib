using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    /// <summary>
    /// http://open.weibo.com/wiki/授权机制说明
    /// </summary>
    public class WeiboOAuthClient : OAuthClient
    {
        public WeiboOAuthClient(OAuthClientSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// http://open.weibo.com/wiki/授权机制说明
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override string ParseAccessToken(string content)
        {
            var data = JsonConverter.FromJson<WeiboAccessToken>(content);
            if (String.IsNullOrEmpty(data.AccessToken))
            {
                throw new Exception("调用新浪微博接口发生异常。\r\n详细信息为：" + content + "\r\n清参考 http://open.weibo.com/wiki/授权机制说明");
            }
            return data.AccessToken;
        }

        /// <summary>
        /// http://open.weibo.com/wiki/2/account/get_uid
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override string ParseOpenId(string content)
        {
            var data = JsonConverter.FromJson<WeiboOpenId>(content);
            if (String.IsNullOrEmpty(data.OpenId))
            {
                throw new Exception("调用新浪微博接口发生异常。\r\n详细信息为：" + content + "\r\n清参考 http://open.weibo.com/wiki/Error_codec");
            }
            return data.OpenId;
        }

        class WeiboAccessToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
        }

        class WeiboOpenId
        {
            [JsonProperty("uid")]
            public string OpenId { get; set; }
        }
    }
}
