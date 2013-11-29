using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class AuthorizeCodeRequest
    {
        public int ClientId { get; set; }

        public Uri RedirectUri { get; set; }

        public string State { get; set; }

        public string Scope { get; set; }

        public long PlatCode { get; set; }

        public string ExtendField { get; set; }

        public void Parse(HttpRequestBase request)
        {
            ClientId = MessageUtil.GetInt32(request, Protocal.client_id);
            ResponseType responseType;
            if (!ResponseTypeExtend.TryParse(MessageUtil.GetString(request, Protocal.response_type), out responseType))
                throw new OAuthException(AuthorizationRequestErrorCodes.UnsupportedResponseType, "invalid response type", 400);

            RedirectUri = new Uri(MessageUtil.GetString(request, Protocal.redirect_uri));
            State = MessageUtil.TryGetString(request, Protocal.state);
            Scope = MessageUtil.TryGetString(request, Protocal.scope);
            PlatCode = MessageUtil.GetInt64(request, "platcode");
            ExtendField = MessageUtil.TryGetString(request, "extendfield");
        }
    }

    public class AuthorizeCodeRequestDoc
    {
        /// <summary>
        /// 应用的app_key
        /// </summary>
        public int client_id { get; set; }

        /// <summary>
        /// 授权回调地址，必须与注册时的地址一致
        /// </summary>
        public string redirect_uri { get; set; }

        /// <summary>
        /// 必须为"code"
        /// </summary>
        public string response_type { get; set; }

        /// <summary>
        /// 用于保持请求和回调的状态，授权请求成功后原样带回给第三方。该参数用于防止csrf攻击（跨站请求伪造攻击），强烈建议第三方带上该参数。参数设置建议为简单随机数+session的方式
        /// </summary>
        [Optional]
        public string state { get; set; }

        /// <summary>
        /// 授权的范围
        /// </summary>
        [Optional]
        public string scope { get; set; }

        /// <summary>
        /// 平台的代码，参见 <a href="http://wiki.ty.nd/index.php?title=%E5%B9%B3%E5%8F%B0%E4%BB%A3%E7%A0%81">平台代码</a>
        /// </summary>
        public long platcode { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        [Optional]
        public string extendfield { get; set; }
    }
}
