using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class AuthorizePassport91Request : AuthorizeRequestBase
    {
        public long Passport91Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string CookieOrdernumberMaster { get; set; }

        public string CookieOrdernumberSlave { get; set; }

        public string CookieCheckcode { get; set; }

        public string CookieSiteflag { get; set; }

        public override AccountType AccountType
        {
            get { return AccountType.Passport91; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            Passport91Id = MessageUtil.GetInt64(request, "passport91id");
            UserName = MessageUtil.GetString(request, Protocal.username);
            Password = MessageUtil.GetString(request, Protocal.password);
            CookieOrdernumberMaster = MessageUtil.GetString(request, "CookieOrdernumberMaster");
            CookieOrdernumberSlave = MessageUtil.GetString(request, "CookieOrdernumberSlave");
            CookieCheckcode = MessageUtil.GetString(request, "CookieCheckcode");
            CookieSiteflag = MessageUtil.GetString(request, "CookieSiteflag");
        }

        public override AuthorizationCode Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidPassport91(Passport91Id, UserName, Password, CookieOrdernumberMaster, CookieOrdernumberSlave, CookieCheckcode, CookieSiteflag, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, 400);

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }

    }
}
