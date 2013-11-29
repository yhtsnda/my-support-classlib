using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// An indication of what kind of response the client is requesting from the authorization server
    /// after the user has granted authorized access.
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// An access token should be returned immediately.
        /// </summary>
        AccessToken,

        /// <summary>
        /// An authorization code should be returned, which can later be exchanged for refresh and access tokens.
        /// </summary>
        AuthorizationCode
    }

    internal static class ResponseTypeExtend
    {
        public static bool TryParse(string value, out ResponseType responseType)
        {
            responseType = ResponseType.AuthorizationCode;
            if (String.IsNullOrEmpty(value))
                return false;

            value = value.ToLower();
            switch (value)
            {
                case "token":
                    responseType = ResponseType.AccessToken;
                    return true;
                case "code":
                    responseType = ResponseType.AuthorizationCode;
                    return true;
            }
            return false;
        }

        public static string ToValue(this ResponseType responseType)
        {
            switch (responseType)
            {
                case ResponseType.AccessToken:
                    return "token";
                case ResponseType.AuthorizationCode:
                    return "code";
            }
            return "";
        }
    }
}
