using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuth
{
    /// <summary>
    /// 验证类型
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// 终端用户使用认证码的方式
        /// </summary>
        AuthorizationCode = 0,

        /// <summary>
        /// 终端用户使用密码的方式
        /// </summary>
        Password = 1,

        /// <summary>
        /// 使用客户端凭证的方式
        /// </summary>
        ClientCredentials = 2, 

        /// <summary>
        /// 可刷新凭证
        /// </summary>
        RefreshToken = 3,

        /// <summary>
        /// 使用信任客户端的 AppToken 获得用户的 UserToken
        /// </summary>
        UserToken = 4,
    }

    internal static class GrantTypeExtend
    {
        public static bool TryParse(string value, out GrantType grantType)
        {
            grantType = GrantType.AuthorizationCode;
            if (String.IsNullOrEmpty(value))
                return false;

            value = value.ToLower();
            switch (value)
            {
                case "authorization_code":
                    grantType = GrantType.AuthorizationCode;
                    return true;
                case "password":
                    grantType = GrantType.Password;
                    return true;
                case "refresh_token":
                    grantType = GrantType.RefreshToken;
                    return true;
                case "client_credentials":
                    grantType = GrantType.ClientCredentials;
                    return true;
                case "user_token":
                    grantType = GrantType.UserToken;
                    return true;
            }
            return false;
        }

        public static string ToValue(this GrantType grantType)
        {
            switch (grantType)
            {
                case GrantType.AuthorizationCode:
                    return "authorization_code";
                case GrantType.Password:
                    return "password";
                case GrantType.ClientCredentials:
                    return "client_credentials";
                    break;
                case GrantType.RefreshToken:
                    return "refresh_token";
                case GrantType.UserToken:
                    return "user_token";
            }
            return "";
        }
    }
}
