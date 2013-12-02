using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public abstract class AuthorizeData
    {
        public AuthorizeData(int clientId, string redirectUri, long platCode, string state = null, string scope = null, string browser = null, string ipAddress = null, string extendField = null)
        {
            ClientId = clientId;
            RedirectUri = redirectUri;
            PlatCode = platCode;
            State = state;
            Scope = scope;
            Browser = browser;
            IpAddress = ipAddress;
            ExtendField = extendField;
        }
        public int ClientId;

        public string RedirectUri;

        public string State;

        public string Scope;

        public long PlatCode;

        public string Browser;

        public string IpAddress;

        public string ExtendField;

        public virtual void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.client_id, ClientId.ToString());
            data.Add(Protocal.redirect_uri, RedirectUri);
            data.Add(Protocal.state, State);
            data.Add(Protocal.scope, Scope);
            data.Add(Protocal.response_type, Protocal.code);
            data.Add(Protocal.platcode, PlatCode.ToString());
            data.Add("browser", Browser);
            data.Add("ipaddress", IpAddress);
            data.Add("extendfield", ExtendField);
        }
    }
}
