using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    public class UserTokenData : SimpleTokenData
    {
        public UserTokenData(int clientId, string clientSecret, string accessToken, long userId)
            : base(clientId, clientSecret)
        {
            AccessToken = accessToken;
            UserId = userId;
        }

        public string AccessToken;

        public long UserId;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add(Protocal.ACCESS_TOKEN, AccessToken);
            data.Add("userid", UserId.ToString());
            data.Add(Protocal.GRANT_TYPE, "user_token");

            base.UpdateDatas(data);
        }
    }
}
