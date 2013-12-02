using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public abstract class LoginTokenData : TokenData
    {
        public LoginTokenData(int clientId, string clientSecret, long platCode, string browser = null, string ipAddress = null, string extendField = null)
            : base(clientId, clientSecret)
        {
            PlatCode = platCode;
            Browser = browser;
            IpAddress = ipAddress;
            ExtendField = extendField;
        }

        public long PlatCode;

        public string Browser;

        public string IpAddress;

        public string ExtendField;

        public override void UpdateDatas(NameValueCollection data)
        {
            base.UpdateDatas(data);

            data.Add(Protocal.platcode, PlatCode.ToString());
            data.Add("browser", Browser);
            data.Add("ipaddress", IpAddress);
            data.Add("extendfield", ExtendField);
        }
    }
}
