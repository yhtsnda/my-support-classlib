using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// The types of authorizations that a client can use to obtain
    /// a refresh token and/or an access token.
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// The client is providing the authorization code previously obtained from an end user authorization response.
        /// </summary>
        AuthorizationCode = 0,

        /// <summary>
        /// The client is providing the end user's username and password to the authorization server.
        /// </summary>
        Password = 1,

        /// <summary>
        /// No authorization to access a user's data has been given.  The client is requesting
        /// an access token authorized for its own private data.  This fits the classic OAuth 1.0(a) "2-legged OAuth" scenario.
        /// </summary>
        /// <remarks>
        /// When requesting an access token using the none access grant type (no access grant is included), the client is requesting access to the protected resources under its control, or those of another resource owner which has been previously arranged with the authorization server (the method of which is beyond the scope of this specification).
        /// </remarks>
        ClientCredentials = 2,

        /// <summary>
        /// The client is providing a refresh token.
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
                case GrantType.RefreshToken:
                    return "refresh_token"; 
                case GrantType.UserToken:
                    return "user_token";
            }
            return "";
        }
    }
}
