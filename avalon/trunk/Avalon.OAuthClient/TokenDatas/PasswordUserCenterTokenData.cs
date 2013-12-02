using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class PasswordUserCenterTokenData : LoginTokenData
    {
        public PasswordUserCenterTokenData(int clientId, string clientSecret, long platCode,
            string userName, string password,
            string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, clientSecret, platCode, browser, ipAddress, extendField)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName;

        public string Password;

        public override void UpdateDatas(NameValueCollection data)
        {
            data.Add("username", UserName);
            data.Add("password", Password);
            data.Add(Protocal.grant_type, Protocal.password);
            data.Add(Protocal.account_type, "user_center");

            base.UpdateDatas(data);
        }
    }
}
