using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class TokenPasswordUserCenterRequest : TokenLoginRequest
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public override GrantType GrantType
        {
            get { return GrantType.Password; }
        }

        public AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtil.GetString(request, Protocal.username);
            Password = MessageUtil.GetString(request, Protocal.password);
        }

        public override AccessGrant Token()
        {
            ValidClient();

            var result = OAuthService.ValidPassword(UserName, Password, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, result.Code);

            return OAuthService.CreateAccessGrant(ClientId, ClientCode, result.UserId);
        }
    }
}
