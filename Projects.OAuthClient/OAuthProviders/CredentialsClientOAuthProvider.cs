using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Util;

namespace Projects.OAuthClient
{
    public class CredentialsClientOAuthProvider : AbstractOAuthProvider
    {
        protected override void OnUserAuthorizeSuccess(AccessGrant accessGrant)
        {
            
        }

        protected override AccessGrant GetCurrentUserAccessGrant()
        {
            return null;
        }

        public override Uri OnOpenApiRequest(Uri uri)
        {
            string accessToken = GetCurrent().AppAccessToken;
            return GetUri(uri, accessToken);
        }
    }
}
