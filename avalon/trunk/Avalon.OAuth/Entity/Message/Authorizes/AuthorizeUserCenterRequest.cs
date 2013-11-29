using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class AuthorizeUserCenterRequest : AuthorizeRequestBase
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public override AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtil.GetString(request, Protocal.username);
            Password = MessageUtil.GetString(request, Protocal.password);
        }

        public override AuthorizationCode Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidPassword(UserName, Password, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, 400);

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }
    }
}
