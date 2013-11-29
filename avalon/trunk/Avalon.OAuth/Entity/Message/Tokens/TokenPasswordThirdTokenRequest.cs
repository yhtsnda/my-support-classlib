using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class TokenPasswordThirdTokenRequest : TokenLoginRequest
    {
        public string AccessToken { get; set; }

        public int MappingType { get; set; }

        public override GrantType GrantType
        {
            get { return GrantType.Password; }
        }

        public AccountType AccountType
        {
            get { return AccountType.ThirdToken; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            AccessToken = MessageUtil.GetAccessToken(request);
            MappingType = MessageUtil.GetInt32(request, "mappingtype");
        }

        public override AccessGrant Token()
        {
            ValidClient();

            var result = OAuthService.ValidThirdToken(AccessToken, MappingType, PlatCode, Browser, IpAddress, ExtendField);
            if (result.Code != 0)
                OAuthError(result.Code.ToString(), result.Message, result.Code);

            return OAuthService.CreateAccessGrant(ClientId, ClientCode, result.UserId);
        }
    }
}
