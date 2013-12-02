using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class AuthorizationCodeTokenData : TokenData
    {
        public AuthorizationCodeTokenData(int clientId, string clientSecret,
            string code, string redirectUri)
            : base(clientId, clientSecret)
        {
            Code = code;
            RedirectUri = redirectUri;
        }

        public string Code;

        public string RedirectUri;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.code, Code);
            data.Add(Protocal.redirect_uri, RedirectUri);
            data.Add(Protocal.grant_type, Protocal.authorization_code);
            base.UpdateDatas(data);
        }
    }
}
