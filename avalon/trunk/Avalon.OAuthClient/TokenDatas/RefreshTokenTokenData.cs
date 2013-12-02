using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class RefreshTokenTokenData : TokenData
    {
        public RefreshTokenTokenData(int clientId, string clientSecret,
            string refreshToken)
            : base(clientId, clientSecret)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.refresh_token, RefreshToken);
            data.Add(Protocal.grant_type, Protocal.refresh_token);

            base.UpdateDatas(data);
        }
    }
}
