using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Projects.Tool;
using Projects.Framework;

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

            AccountType accountType;
            if (!AccountTypeExtend.TryParse(GetString(request, Protocal.ACCOUNT_TYPE), out accountType))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong account_type value.", 400);

            AuthRequest authRequest = null;
            if (accountType == AccountType.UserCenter)
                authRequest = new AuthUserCenterRequest();
            else if (accountType == AccountType.ThirdToken)
                authRequest = new AuthThirdRequest();
            else
                throw new OAuthException(AccessTokenRequestErrorCode.UnsupportedAccountType,
                    "account_type:" + accountType.ToValue(), 400);
            authRequest.Parse(request);
            return authRequest;
        }

        /// <summary>
        /// 从HTTP请求中解析凭证的信息
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <returns>凭证请求对象</returns>
        public static TokenRequestBase ParseTokenRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");

            GrantType grantType;
            if (!GrantTypeExtend.TryParse(GetString(request, Protocal.GRANT_TYPE), out grantType))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong grant_type, value", 400);

            TokenRequestBase tokenRequest = null;
            switch (grantType)
            {
                case GrantType.Password:
                    AccountType accountType;
                    if (!AccountTypeExtend.TryParse(GetString(request, Protocal.ACCOUNT_TYPE), out accountType))
                        throw new OAuthException(
                            AccessTokenRequestErrorCode.InvalidRequest, 
                            "wrong account_type value", 
                            400);
                    switch (accountType)
                    {
                        case AccountType.UserCenter:
                            tokenRequest = new TokenPasswordUserCenterRequest();
                            break;
                        case AccountType.ThirdToken:
                            tokenRequest = new TokenPasswordThirdTokenRequest();
                            break;
                        default:
                            throw new OAuthException(
                                AccessTokenRequestErrorCode.UnsupportedAccountType, 
                                "account_type: " + accountType.ToValue(),
                                400);
                    }
                    break;
                case GrantType.ClientCredentials:
                    tokenRequest = new TokenClientCredentialsRequest();
                    break;
                case GrantType.RefreshToken:
                    tokenRequest = new TokenRefreshRequest();
                    break;
                case GrantType.AuthorizationCode:
                    tokenRequest = new TokenAuthrizationCodeRequest();
                    break;
                case GrantType.UserToken:
                    tokenRequest = new TokenUserRequest();
                    break;
                default:
                    throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong grant_type, value", 400);
            }
            tokenRequest.Parse(request);
            return tokenRequest;
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
                throw new OAuthException(BearerTokenErrorCode.InvalidToken, "access token is null", 401);

            return accessToken;
        }

        public static AuthCodeRequest ParseAuthCodeRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");
            var codeRequest = new AuthCodeRequest();
            codeRequest.Parse(request);
            return codeRequest;
        }

        public static string GetString(HttpRequestBase request, string key)
        {
            var valueData = request[key];
            if (String.IsNullOrEmpty(valueData))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, key + " is empty.", 400);
            return valueData;
        }

        public static string TryGetString(HttpRequestBase request, string key)
        {
            var value = request[key];
            return value;
        }

        public static int GetInt32(HttpRequestBase request, string key)
        {
            int value;
            if (!Int32.TryParse(GetString(request, key), out value))
                throw new PlatformException(AccessTokenRequestErrorCode.InvalidRequest, key + " must be int.", 400);
            return value;
        }

        public static long GetInt64(HttpRequestBase request, string key)
        {
            long value;
            if (!Int64.TryParse(GetString(request, key), out value))
                throw new PlatformException(AccessTokenRequestErrorCode.InvalidRequest, key + " must be long.", 400);
            return value;
        }

        public static string GetAccessToken(HttpRequestBase request)
        {
            var accessToken = MessageUtility.TryGetString(request, Protocal.ACCESS_TOKEN);
            if (String.IsNullOrEmpty(accessToken))
                accessToken = MessageUtility.TryGetString(request, Protocal.ACCESSTOKEN);

            if (String.IsNullOrEmpty(accessToken))
                throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, Protocal.ACCESS_TOKEN, 400);

            return accessToken;
        }
    }
}
