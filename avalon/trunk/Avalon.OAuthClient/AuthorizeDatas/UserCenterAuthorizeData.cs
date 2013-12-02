using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public class UserCenterAuthorizeData : AuthorizeData
    {
        public UserCenterAuthorizeData(int clientId, string redirectUri, long platCode,
            string userName, string password,
            string state = null, string scope = null, string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, redirectUri, platCode, state, scope, browser, ipAddress, extendField)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName;

        public string Password;

        public override void UpdateDatas(System.Collections.Specialized.NameValueCollection data)
        {
            data.Add("username", UserName);
            data.Add("password", Password);
            data.Add(Protocal.account_type, "user_center");

            base.UpdateDatas(data);
        }
    }
}
