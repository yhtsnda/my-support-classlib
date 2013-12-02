using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 支持通过 userid 来获取UserAccessToken
    /// </summary>
    public class UserCredentialsClientOAuthProvider : CredentialsClientOAuthProvider
    {
        ICache cache = new AspnetCache() { CacheName = "OAuth" };
        const string UserCredentialsClientFormat = "_ucc_{0}_{1}";

        protected override AccessGrant GetCurrentAccessGrant()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return base.GetCurrentAccessGrant();

            var key = String.Format(UserCredentialsClientFormat, AppMetadata.ClientId, userId);

            var accessGrant = cache.Get<AccessGrant>(key);
            if (accessGrant != null)
                return accessGrant;

            AccessGrant appAccessGrant = base.GetCurrentAccessGrant();
            EnsureAppMetadata();
            var am = AppMetadata;
            UserTokenTokenData token = new UserTokenTokenData(am.ClientId, am.ClientSecret, appAccessGrant.AccessToken, userId);
            return AuthorizeAndValid(token);
        }

        protected override void OnAuthorizeSuccess(AccessGrant accessGrant)
        {
            if (accessGrant.UserId != 0)
                cache.Set(String.Format(UserCredentialsClientFormat,accessGrant.ClientId, accessGrant.UserId), accessGrant);

            base.OnAuthorizeSuccess(accessGrant);
        }

        protected virtual long GetCurrentUserId()
        {
            var context = HttpContext.Current;
            if (context.IsAvailable())
            {
                var formsIdentity = context.User.Identity as FormsIdentity;
                if (formsIdentity != null && formsIdentity.IsAuthenticated)
                    return Int64.Parse(formsIdentity.Name);
            }
            return 0;
        }
    }
}
