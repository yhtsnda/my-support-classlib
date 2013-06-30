using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 授权过程中的约定的协议常量
    /// </summary>
    internal class Protocal
    {
        /// <summary>
        /// The "state" string.
        /// </summary>
        internal const string STATE = "state";

        /// <summary>
        /// The "redirect_uri_mismatch" string.
        /// </summary>
        internal const string REDIRECT_URI_MISMATCH = "redirect_uri_mismatch";

        /// <summary>
        /// The "redirect_uri" string.
        /// </summary>
        internal const string REDIRECT_URI = "redirect_uri";

        /// <summary>
        /// The "client_id" string.
        /// </summary>
        internal const string CLIENT_ID = "client_id";

        /// <summary>
        /// The "scope" string.
        /// </summary>
        internal const string SCOPE = "scope";

        /// <summary>
        /// The "client_secret" string.
        /// </summary>
        internal const string CLIENT_SECRET = "client_secret";

        /// <summary>
        /// The "token" string
        /// </summary>
        internal const string TOKEN = "token";

        /// <summary>
        /// The "code" string.
        /// </summary>
        internal const string CODE = "code";

        /// <summary>
        /// The "error" string.
        /// </summary>
        internal const string ERROR = "error";

        /// <summary>
        /// The "access_token" string.
        /// </summary>
        internal const string ACCESS_TOKEN = "access_token";

        /// <summary>
        /// The "access_token" string.
        /// </summary>
        /// <remarks>
        /// 旧有OAUTH中的AccessToken字符串
        /// </remarks>
        internal const string OLD_ACCESS_TOKEN = "accesstoken";

        /// <summary>
        /// The "token_type" string.
        /// </summary>
        internal const string TOKEN_TYPE = "token_type";

        /// <summary>
        /// The "refresh_token" string.
        /// </summary>
        internal const string REFRESH_TOKEN = "refresh_token";

        /// <summary>
        /// The "expires_in" string.
        /// </summary>
        internal const string EXPIRES_IN = "expires_in";

        /// <summary>
        /// The "username" string.
        /// </summary>
        internal const string USER_NAME = "username";

        /// <summary>
        /// The "password" string.
        /// </summary>
        internal const string PASSWORD = "password";

        /// <summary>
        /// The "platcode" string.
        /// </summary>
        public const string PLATCODE = "platcode";

        /// <summary>
        /// The "third_token" string.
        /// </summary>
        public const string THIRD_TOKEN = "third_token";

        /// <summary>
        /// The "error_uri" string.
        /// </summary>
        internal const string ERROR_URI = "error_uri";

        /// <summary>
        /// The "error_description" string.
        /// </summary>
        internal const string ERROR_DESCRIPTION = "error_description";

        /// <summary>
        /// The "response_type" string.
        /// </summary>
        internal const string RESPONSE_TYPE = "response_type";

        /// <summary>
        /// The "grant_type" string.
        /// </summary>
        internal const string GRANT_TYPE = "grant_type";

        /// <summary>
        /// The "account_type" string.
        /// </summary>
        internal const string ACCOUNT_TYPE = "account_type";

        /// <summary>
        /// The "authorization_code" string.
        /// </summary>
        internal const string AUTHORIZATION_CODE = "authorization_code";

        /// <summary>
        /// The "client_credentials" string.
        /// </summary>
        internal const string CLIENT_CREDENTIALS = "client_credentials";
    }
}
