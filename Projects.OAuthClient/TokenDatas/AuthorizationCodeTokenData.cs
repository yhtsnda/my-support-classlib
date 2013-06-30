using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    public class AuthorizationCodeTokenData : SimpleTokenData
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
            data.Add(Protocal.CODE, Code);
            data.Add(Protocal.REDIRECT_URI, RedirectUri);
            data.Add(Protocal.GRANT_TYPE, Protocal.AUTHORIZATION_CODE);
            base.UpdateDatas(data);
        }
    }
}
