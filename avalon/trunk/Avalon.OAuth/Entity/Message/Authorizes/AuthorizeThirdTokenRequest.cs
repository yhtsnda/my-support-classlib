using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class AuthorizeThirdTokenRequest : AuthorizeRequestBase
    {
        public string AccessToken { get; set; }

        public int MappingType { get; set; }

        public override AccountType AccountType
        {
            get { return AccountType.ThirdToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            AccessToken = MessageUtil.ParseAccessToken(request);
            MappingType = MessageUtil.GetInt32(request, "mappingtype");
        }

        public override AuthorizationCode Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidThirdToken(AccessToken, MappingType, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, 400);

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }
    }
}
