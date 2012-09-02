using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Projects.Tool.Util
{
    public static class IpAddress
    {
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;
            if (HttpContext.Current == null)
                return result;

            result = HttpContext.Current.Request.Headers["X-Real-IP"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result))
                return "0.0.0.0";

            return result;
        }
    }
}
