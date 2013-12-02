using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class UserTokenTokenData : TokenData
    {
        public UserTokenTokenData(int clientId, string clientSecret, 
            string accessToken, long userId)
            : base(clientId, clientSecret)
        {
            AccessToken = accessToken;
            UserId = userId;
        }

        public string AccessToken;

        public long UserId;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.accesstoken, AccessToken);
            data.Add("userid", UserId.ToString());
            data.Add(Protocal.grant_type, "user_token");

            base.UpdateDatas(data);
        }
    }
}
