using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// 检查是否是一个有效的IP地址
        /// </summary>
        /// <param name="checkString">要检查的IP地址</param>
        /// <param name="haveWildcard">是否包含有通配符</param>
        /// <returns></returns>
        public static bool ValidateIP(string checkString, bool haveWildcard)
        {
            string reg1 = @"(\d\d?|2[0-4]\d|25[0-5])\.(\d\d?|2[0-4]\d|25[0-5])\.(\d\d?|2[0-4]\d|25[0-5]|\*)\.(\d\d?|2[0-4]\d|25[0-5]|\*)(?x)";
            string reg2 = @"(\d\d?|2[0-4]\d|25[0-5])\.(\d\d?|2[0-4]\d|25[0-5])\.(\d\d?|2[0-4]\d|25[0-5])\.(\d\d?|2[0-4]\d|25[0-5])(?x)";

            Regex regex = null;
            if (haveWildcard)
                regex = new Regex(reg1);
            else
                regex = new Regex(reg2);

            return regex.IsMatch(checkString);
        }
    }
}
