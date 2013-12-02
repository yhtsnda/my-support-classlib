using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.UCenter
{
    /// <summary>
    /// 应用管理地址：http://connect.qq.com/manage/detail?appid=100282952&platform=web
    /// http://wiki.open.qq.com/wiki/website/OAuth2.0%E5%BC%80%E5%8F%91%E6%96%87%E6%A1%A3
    /// </summary>
    public class TencentOAuthClient : OAuthClient
    {
        public TencentOAuthClient(OAuthClientSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// http://wiki.open.qq.com/wiki/website/使用Authorization_Code获取Access_Token
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override string ParseAccessToken(string content)
        {
            var values = HttpUtility.ParseQueryString(content, Encoding.UTF8);
            var access_token = values["access_token"];
            if (String.IsNullOrEmpty(access_token))
            {
                throw new Exception("调用腾讯接口发生异常。\r\n详细信息为：" + content + "\r\n清参考 http://wiki.open.qq.com/wiki/website/公共返回码说明");
            }
            return access_token;
        }

        /// <summary>
        /// http://wiki.open.qq.com/wiki/website/获取用户OpenID_OAuth2.0
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override string ParseOpenId(string content)
        {
            if (content.StartsWith("callback("))
            {
                var v = content.Substring(9, content.Length - 9 - 3);
                var data = JsonConverter.FromJson<TencentOpenId>(v);
                return data.OpenId;
            }
            throw new Exception("调用腾讯接口发生异常。\r\n详细信息为：" + content + "\r\n清参考 http://wiki.open.qq.com/wiki/website/获取用户OpenID_OAuth2.0");
        }

        class TencentOpenId
        {
            [JsonProperty("openid")]
            public string OpenId { get; set; }
        }
    }
}
