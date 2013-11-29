using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    /// <summary>
    /// 授权码获取访问许可请求
    /// </summary>
    public class TokenAuthorizationCodeRequest : TokenRequestBase
    {
        public Uri RedirectUri { get; private set; }

        public string Code { get; private set; }

        public override GrantType GrantType
        {
            get { return GrantType.AuthorizationCode; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            Code = MessageUtil.GetString(request, Protocal.code);
            RedirectUri = new Uri(MessageUtil.GetString(request, Protocal.redirect_uri));
        }

        public override AccessGrant Token()
        {
            ValidClient();

            var code = OAuthService.GetAuthorizationCode(Code);
            if (code == null)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "code invalid");

            if (code.IsExpire())
            {
                OAuthService.DeleteAuthorizationCode(code);
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "code expire");
            }

            if (code.AppId != ClientId)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "client id is not match.", 400);

            OAuthService.DeleteAuthorizationCode(code);

            return OAuthService.CreateAccessGrant(ClientId, ClientCode, code.UserId);
        }
    }

    public class TokenAuthorizationCodeRequestDoc : TokenRequestBaseDoc
    {
        /// <summary>
        /// 必须为 "authorization_code"
        /// </summary>
        public string grant_type { get; set; }

        /// <summary>
        /// 调用authorize时返回的code
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 授权回调地址，必须与注册时的地址一致
        /// </summary>
        public string redirect_uri { get; set; }
    }
}
