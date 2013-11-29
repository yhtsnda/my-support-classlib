using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class TokenRefreshRequest : TokenRequestBase
    {
        public string RefreshToken { get; private set; }

        public override GrantType GrantType
        {
            get { return GrantType.RefreshToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            RefreshToken = MessageUtil.GetString(request, Protocal.refresh_token);
        }

        public override AccessGrant Token()
        {
            ValidClient();

            AccessGrant accessGrant = OAuthService.GetAccessGrantByRefreshToken(RefreshToken);

            if (accessGrant == null)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "refresh token invalid", 400);

            if (ClientId != accessGrant.ClientId)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "client id is not match.", 400);

            if (accessGrant.IsRefreshExpire())
            {
                OAuthService.DeleteAccessGrant(accessGrant);
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "refresh token expire", 400);
            }

            var refreshedToken = new AccessGrant(accessGrant.ClientId, accessGrant.ClientCode, accessGrant.UserId)
            {
                Scope = accessGrant.Scope,
                GrantType = accessGrant.GrantType
            };

            OAuthService.CreateAccessGrant(refreshedToken);
            OAuthService.DeleteAccessGrant(accessGrant);

            return refreshedToken;
        }


    }
}
