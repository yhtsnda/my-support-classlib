using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Projects.Tool.Http;

namespace Projects.OAuthClient
{
    /// <summary>
    /// Web的辅助类
    /// </summary>
    public class HttpUtilities
    {
        public static string GetRequestBrowser()
        {
            var browserCode = "unknown";
            var context = HttpContext.Current;

            if(context.IsAvailable() && context.Request.Browser != null)
                browserCode = context.Request.Browser.Browser;
            return browserCode;
        }

        public static string GetRequestIp()
        {
            return IpAddress.GetIP();
        }
    }
}
