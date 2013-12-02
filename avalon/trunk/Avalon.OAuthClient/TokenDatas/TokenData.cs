using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public abstract class TokenData
    {
        public TokenData(int clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public int ClientId;

        public string ClientSecret;

        public virtual void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.client_id, ClientId.ToString());
            data.Add(Protocal.client_secret, ClientSecret);
            data.Add(Protocal.response_type, Protocal.token);
        }
    }
}
