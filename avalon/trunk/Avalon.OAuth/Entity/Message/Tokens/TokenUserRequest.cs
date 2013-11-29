using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class TokenUserRequest : TokenRequestBase
    {
        public long UserId { get; set; }

        public string AccessToken { get; set; }

        public override GrantType GrantType
        {
            get { return GrantType.UserToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            UserId = MessageUtil.GetInt64(request, "userid");
            AccessToken = MessageUtil.GetAccessToken(request);
        }

        public override AccessGrant Token()
        {
            ValidClient();

            var accessGrant = OAuthService.GetAccessGrant(AccessToken);
            if (accessGrant == null)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "invalid access token.");
            if (accessGrant.IsExpire())
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "access token expired.", OAuthService.TokenExpireCode);

            if (UserId == 0)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "invalid userid");

            return OAuthService.CreateAccessGrant(accessGrant.ClientId, accessGrant.ClientCode, UserId);
        }
    }
}
