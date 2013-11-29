using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class TokenClientCredentialsRequest : TokenRequestBase
    {
        public override GrantType GrantType
        {
            get { return GrantType.ClientCredentials; }
        }

        public override AccessGrant Token()
        {
            ValidClient();
            return OAuthService.CreateAccessGrant(ClientId);
        }
    }
}
