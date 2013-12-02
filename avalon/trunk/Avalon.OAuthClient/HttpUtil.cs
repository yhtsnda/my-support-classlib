using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// Web 服务器辅助类
    /// </summary>
    public class HttpUtil
    {
        /// <summary>
        /// Gets the request browser.
        /// </summary>
        /// <returns></returns>
        public static string GetRequestBrowser()
        {
            var browserCode = "unknown";
            var context = HttpContext.Current;
            if (context.IsAvailable() && context.Request.Browser != null)
            {
                browserCode = context.Request.Browser.Browser;
            }
            return browserCode;
        }

        /// <summary>
        /// Gets the request ip address.
        /// </summary>
        /// <returns></returns>
        public static string GetRequestIpAddress()
        {
            return Avalon.Utility.IpAddress.GetIP();
        }
    }
}
