using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Projects.Tool;
using Projects.Framework;
using Projects.OAuth.Message;

namespace Projects.OAuth
{
    /// <summary>
    /// 授权消息处理类
    /// </summary>
    internal class MessageUtility
    {
        /// <summary>
        /// 从Http请求中解析授权请求的信息
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>授权请求对象</returns>
        public static AuthRequest ParseAuthRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");

            int clientId = GetClientId(request);
            Uri redirectUri = GetRedirectUri(request);

            AuthResponseType responseType;
            if (!AuthResponseTypeExtend.TryParse(GetValue(request, Protocal.RESPONSE_TYPE), out responseType))
                throw new OAuthException(AuthorizationRequestErrorCodes.InvalidRequest, "response_type error", 400);

            string state = request[Protocal.STATE];
            return new AuthRequest(clientId, responseType, redirectUri, state);
        }

        /// <summary>
        /// 从HTTP请求中解析凭证的信息
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>凭证请求对象</returns>
        public static TokenRequestBase ParseTokenRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");

            int clientId = GetClientId(request);
            var clientSecret = GetValue(request, Protocal.CLIENT_SECRET);

            GrantType grantType;
            if (!GrantTypeEntend.TryParse(GetValue(request, Protocal.GRANT_TYPE), out grantType))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong grant_type, value", 400);

            switch (grantType)
            {
                case GrantType.Password:
                    var userName = GetValue(request, Protocal.USER_NAME);
                    var password = GetValue(request, Protocal.PASSWORD);
                    return new TokenPasswordCredentialsRequest(clientId, clientSecret, userName, password);
                case GrantType.ClientCredentials:
                    return new TokenClientCredentialsRequest(clientId, clientSecret);
                case GrantType.AuthorizationCode:
                    var code = GetValue(request, Protocal.CODE);
                    Uri uri = GetRedirectUri(request);
                    return new TokenAuthrizationCodeRequest(clientId, clientSecret, code, uri);
                case GrantType.RefreshToken:
                    var refreshToken = GetValue(request, Protocal.REFRESH_TOKEN);
                    return new TokenRefreshRequest(clientId, clientSecret, refreshToken);
                default:
                    throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong grant_type, value", 400);
            }
        }

        /// <summary>
        /// 从HTTP请求中解析授权凭证
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>授权凭证字符串</returns>
        public static string ParseAccessToken(HttpRequestBase request)
        {
            var accessToken = request[Protocal.ACCESS_TOKEN];
            
            //这里考虑处理之前的OAuth类中的AccessToken
            if (String.IsNullOrWhiteSpace(accessToken))
                accessToken = request[Protocal.OLD_ACCESS_TOKEN];

            if (String.IsNullOrWhiteSpace(accessToken))
                throw new OAuthException(BearerTokenError.InvalidToken, "access token is null", 400);

            return accessToken;
        }

        private static int GetClientId(HttpRequestBase request)
        {
            int clientId;
            if(!Int32.TryParse(GetValue(request, Protocal.CLIENT_ID), out clientId))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "client_id must be int.", 400);
            return clientId;
        }

        private static Uri GetRedirectUri(HttpRequestBase request)
        {
            Uri redirectUri;
            if (!Uri.TryCreate(GetValue(request, Protocal.REDIRECT_URI), UriKind.RelativeOrAbsolute, out redirectUri))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "redirect_uri format error.", 400);
            return redirectUri;
        }

        private static string GetValue(HttpRequestBase request, string key)
        {
            string value = request[key];
            if (String.IsNullOrEmpty(value))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, key, 400);
            return value;
        }
    }
}
