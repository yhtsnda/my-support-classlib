using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    internal class MessageUtil
    {
        public static AuthorizeCodeRequest ParseAuthorizeCodeRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");
            var codeRequest = new AuthorizeCodeRequest();
            codeRequest.Parse(request);
            return codeRequest;
        }

        /// <summary>
        /// 解析并获取授权请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static AuthorizeRequestBase ParseAuthorizeRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");

            AccountType accountType;
            if (!AccountTypeExtend.TryParse(GetString(request, Protocal.account_type), out accountType))
                throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, "wrong account_type value", 400);

            AuthorizeRequestBase authorizationRequest = null;
            switch (accountType)
            {
                case AccountType.Passport91:
                    authorizationRequest = new AuthorizePassport91Request();
                    break;
                case AccountType.ThirdToken:
                    authorizationRequest = new AuthorizeThirdTokenRequest();
                    break;
                case AccountType.UserCenter:
                    authorizationRequest = new AuthorizeUserCenterRequest();
                    break;
                default:
                    throw new OAuthException(AccessTokenRequestErrorCodes.UnsupportedAccountType, "account_type: " + accountType.ToValue(), 400);
            }
            authorizationRequest.Parse(request);
            return authorizationRequest;
        }

        /// <summary>
        /// 解析并获取访问许可请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TokenRequestBase ParseTokenRequest(HttpRequestBase request)
        {
            Arguments.NotNull(request, "request");

            GrantType grantType;
            if (!GrantTypeExtend.TryParse(GetString(request, Protocal.grant_type), out grantType))
                throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, "wrong grant_type value", 400);

            TokenRequestBase tokenRequest = null;
            switch (grantType)
            {
                case GrantType.Password:
                    AccountType accountType;
                    if (!AccountTypeExtend.TryParse(GetString(request, Protocal.account_type), out accountType))
                        throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, "wrong account_type value", 400);
                    switch (accountType)
                    {
                        case AccountType.Passport91:
                            tokenRequest = new TokenPasswordPassport91Request();
                            break;
                        case AccountType.UserCenter:
                            tokenRequest = new TokenPasswordUserCenterRequest();
                            break;
                        case AccountType.ThirdToken:
                            tokenRequest = new TokenPasswordThirdTokenRequest();
                            break;
                        default:
                            throw new OAuthException(AccessTokenRequestErrorCodes.UnsupportedAccountType, "account_type: " + accountType.ToValue(), 400);
                    }
                    break;
                case GrantType.ClientCredentials:
                    tokenRequest = new TokenClientCredentialsRequest();
                    break;
                case GrantType.RefreshToken:
                    tokenRequest = new TokenRefreshRequest();
                    break;
                case GrantType.AuthorizationCode:
                    tokenRequest = new TokenAuthorizationCodeRequest();
                    break;
                case GrantType.UserToken:
                    tokenRequest = new TokenUserRequest();
                    break;
                default:
                    throw new OAuthException(AccessTokenRequestErrorCodes.UnsupportedGrantType, "grant_type: " + grantType.ToValue(), 400);
            }
            tokenRequest.Parse(request);
            return tokenRequest;
        }

        /// <summary>
        /// 解析并获取访问令牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ParseAccessToken(HttpRequestBase request)
        {
            var accessToken = TryParseAccessToken(request);

            if (String.IsNullOrWhiteSpace(accessToken))
                throw new OAuthException(BearerTokenErrorCodes.InvalidToken, "access_token null", 401);

            return accessToken;
        }

        public static string TryParseAccessToken(HttpRequestBase request)
        {
            var accessToken = request[Protocal.access_token];

            //兼容之前的接口
            if (String.IsNullOrWhiteSpace(accessToken))
                accessToken = request[Protocal.accesstoken];

            return accessToken;
        }


        public static string GetString(HttpRequestBase request, string key)
        {
            var value = request[key];
            if (String.IsNullOrEmpty(value))
                throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, key + " is empty.", 400);

            return value;
        }

        public static int GetInt32(HttpRequestBase request, string key)
        {
            int value;
            if (!Int32.TryParse(GetString(request, key), out value))
                throw new AvalonException(AccessTokenRequestErrorCodes.InvalidRequest, key + " must be int.", 400);
            return value;
        }

        public static long GetInt64(HttpRequestBase request, string key)
        {
            long value;
            if (!Int64.TryParse(GetString(request, key), out value))
                throw new AvalonException(AccessTokenRequestErrorCodes.InvalidRequest, key + "must be long.", 400);
            return value;
        }

        public static string TryGetString(HttpRequestBase request, string key)
        {
            var value = request[key];
            return value;
        }

        public static string GetAccessToken(HttpRequestBase request)
        {
            var accessToken = MessageUtil.TryGetString(request, Protocal.access_token);
            if (String.IsNullOrEmpty(accessToken))
                accessToken = MessageUtil.TryGetString(request, Protocal.accesstoken);

            if (String.IsNullOrEmpty(accessToken))
                throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, Protocal.access_token, 400);

            return accessToken;
        }
    }
}

