using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public abstract class TokenLoginRequest : TokenRequestBase
    {
        public long PlatCode { get; private set; }

        public string Browser { get; private set; }

        public string IpAddress { get; private set; }

        public string ExtendField { get; private set; }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            PlatCode = MessageUtil.GetInt64(request, "platcode");
            Browser = MessageUtil.TryGetString(request, "browser");
            IpAddress = MessageUtil.TryGetString(request, "ipaddress");
            ExtendField = MessageUtil.TryGetString(request, "extendfield");

            if (String.IsNullOrEmpty(Browser))
                Browser = GetBrowser(request);
            if (String.IsNullOrEmpty(IpAddress))
                IpAddress = Avalon.Utility.IpAddress.GetIP();
        }

        string GetBrowser(HttpRequestBase request)
        {
            var browserCode = "";
            if (request.Browser != null)
            {
                browserCode = request.Browser.Browser;
            }
            return browserCode;
        }
    }
}
