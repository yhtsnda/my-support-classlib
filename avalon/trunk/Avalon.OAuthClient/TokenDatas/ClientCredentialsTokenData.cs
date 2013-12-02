using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class ClientCredentialsTokenData : TokenData
    {
        public ClientCredentialsTokenData(int clientId, string clientSecret)
            : base(clientId, clientSecret)
        {
        }

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.grant_type, Protocal.client_credentials);

            base.UpdateDatas(data);
        }
    }
}
