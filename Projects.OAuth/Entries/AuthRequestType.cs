using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 验证请求的类型
    /// </summary>
    public enum AuthResponseType
    {
        /// <summary>
        /// 存储凭证
        /// </summary>
        AccessToken,

        /// <summary>
        /// 验证码
        /// </summary>
        AuthorizationCode
    }

    internal static class AuthResponseTypeExtend
    {
        public static bool TryParse(string value, out AuthResponseType responseType)
        {
            responseType = AuthResponseType.AuthorizationCode;
            if (String.IsNullOrEmpty(value))
                return false;

            value = value.ToLower();
            switch (value)
            {
                case "token":
                    responseType = AuthResponseType.AccessToken;
                    return true;
                case "code":
                    responseType = AuthResponseType.AuthorizationCode;
                    return true;
            }
            return false;
        }

        public static string ToValue(this AuthResponseType responseType)
        {
            switch (responseType)
            {
                case AuthResponseType.AccessToken:
                    return "token";
                case AuthResponseType.AuthorizationCode:
                    return "code";
            }
            return "";
        }
    }
}
