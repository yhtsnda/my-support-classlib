using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Web;

using Projects.Framework;
using Projects.Tool;

namespace Projects.OAuth
{
    public class OAuthService : IService
    {
        IClientAuthRepository clientRepository;
        IServerAccessGrantRepository serverAccessRepository;
        IUserProxyRepository userProxRepository;
        IAuthorizationCodeRepository authCodeRepository;
        CacheDomain<ServerAccessGrant, string> serverAccessGrantCache;

        public const string SERVER_ACCESSGRANT_CACHENAME = "accessgrant";
        public const string SERVER_ACCESS_GRANT_CACHEFORMAT = "accessgrant:{0}";

        public OAuthService(IClientAuthRepository clientRepository,
            IServerAccessGrantRepository serverAccessRepository,
            IUserProxyRepository userProxRepository,
            IAuthorizationCodeRepository authCodeRepository)
        {
            this.clientRepository = clientRepository;
            this.serverAccessRepository = serverAccessRepository;
            this.userProxRepository = userProxRepository;
            this.authCodeRepository = authCodeRepository;

            serverAccessGrantCache = CacheDomain.CreateSingleKey<ServerAccessGrant, string>(
                    o => o.AccessToken,
                    GetServerAccessGrantInner,
                    null,
                    SERVER_ACCESSGRANT_CACHENAME,
                    SERVER_ACCESS_GRANT_CACHEFORMAT);
        }

        /// <summary>
        /// 将普通HTTP请求转换为验证请求输入
        /// </summary>
        /// <param name="context">请求的上下文对象</param>
        /// <param name="userId">用户的ID</param>
        /// <param name="authRequest">[输入]验证请求</param>
        /// <returns>服务端授权对象</returns>
        public virtual object Auth(HttpContextBase context, int userId, out AuthRequest authRequest)
        {
            return Auth(context.Request, userId, null, out authRequest);
        }

        /// <summary>
        /// 将普通HTTP请求转换为验证请求输入(使用授权码)
        /// </summary>
        /// <param name="context">请求的上下文对象</param>
        /// <param name="userId">用户的ID</param>
        /// <returns>服务端授权对象</returns>
        public virtual AuthorizationCode AuthByCode(HttpContextBase context, int userId)
        {
            AuthRequest authRequest;
            return (AuthorizationCode)Auth(context.Request, userId, 
                AuthResponseType.AuthorizationCode, out authRequest);
        }



        /// <summary>
        /// 将普通HTTP请求转换为验证请求输入
        /// </summary>
        /// <param name="request">验证请求</param>
        /// <param name="userId">用户的ID</param>
        /// <param name="validType">授权验证类型</param>
        /// <param name="authRequest">[输入]验证请求</param>
        /// <returns>服务端授权对象</returns>
        private object Auth(HttpRequestBase request, int userId, AuthResponseType? validType, out AuthRequest authRequest)
        {
            Arguments.NotNull(request, "request");
            authRequest = MessageUtility.ParseAuthRequest(request);

            var client = clientRepository.Get(authRequest.ClientId);
            if (client == null)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient,
                    "client id invalid.");
            if (!String.Equals(authRequest.RedirectUri.AbsolutePath, client.CallbackPath,
                StringComparison.InvariantCultureIgnoreCase))
                OAuthError(AccessTokenRequestErrorCode.RedirectUriMismatch,
                    "callback uri mismatch.");
            if (validType.HasValue && validType.Value != authRequest.ResponseType)
                OAuthError(AccessTokenRequestErrorCode.UnsupportedResponseType, 
                    "response type muse be " + validType.Value.ToString() + ".");

            switch (authRequest.ResponseType)
            {
                case AuthResponseType.AccessToken:
                    return CreateServerAccessGrant(authRequest.ClientId, userId);
                case AuthResponseType.AuthorizationCode:
                    AuthorizationCode code = new AuthorizationCode(authRequest.ClientId, userId);
                    authCodeRepository.Create(code);
                    return code;
            }

            OAuthError(AccessTokenRequestErrorCode.UnsupportedGrantType, "unknown response type.");
            return null;
        }

        /// <summary>
        /// 从HTTP请求中获取Token
        /// </summary>
        /// <param name="context">请求</param>
        /// <returns>Token对象</returns>
        public virtual ServerAccessGrant Token(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var request = context.Request;
            var tokenRequest = MessageUtility.ParseTokenRequest(context.Request);

            var client = GetClientAuth(tokenRequest.ClientId);

            if (client == null)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client id invalid.");

            if (tokenRequest.ClientSecret != client.Secret)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client secret invalid.");

            if (client.Status != ClientAuthStatus.Enabled)
                OAuthError(AccessTokenRequestErrorCode.UnauthorizedClient, "client unauthorized", 401);

            ServerAccessGrant accessGrant = null;
            switch (tokenRequest.GrantType)
            {
                case GrantType.AuthorizationCode:
                    accessGrant = Token((TokenAuthrizationCodeRequest)tokenRequest);
                    break;
                case GrantType.ClientCredentials:
                    accessGrant = Token((TokenClientCredentialsRequest)tokenRequest);
                    break;
                case GrantType.Password:
                    accessGrant = Token((TokenPasswordCredentialsRequest)tokenRequest);
                    break;
                case GrantType.RefreshToken:
                    accessGrant = Token((TokenRefreshRequest)tokenRequest);
                    break;
            }
            return accessGrant;
        }

        private ServerAccessGrant Token(TokenAuthrizationCodeRequest request)
        {
            var code = authCodeRepository.Get(request.Code);
            if(code == null)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "code invalid");
            if(code.AppId != request.ClientId)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "client id is not match.", 400);

            //删除原有的授权码
            authCodeRepository.Delete(code);
            //创建新的服务端授权
            return CreateServerAccessGrant(request.ClientId, code.UserId);
        }

        private ServerAccessGrant Token(TokenClientCredentialsRequest request)
        {
            return CreateServerAccessGrant(request.ClientId);
        }

        private ServerAccessGrant Token(TokenPasswordCredentialsRequest request)
        {
            var user = GetUserProxy(request.UserName);
            if (user == null || user.Password != request.Password)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "username or password invalid");
            return CreateServerAccessGrant(request.ClientId, user.UserId);
        }

        private ServerAccessGrant Token(TokenRefreshRequest request)
        {
            ServerAccessGrant accessGrant = GetServerAccessGrantByRefreshToken(request.RefreshToken);

            if(accessGrant == null)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "refresh token invalid", 400);
            if(request.ClientId != accessGrant.ClientId)
                OAuthError(AccessTokenRequestErrorCode.InvalidRequest, "client id is not match.", 400);
            var refreshedToken = new ServerAccessGrant(accessGrant.ClientId, accessGrant.UserId)
            {
                Scope = accessGrant.Scope,
                GrantType = accessGrant.GrantType
            };
            CreateServerAccessGrant(refreshedToken);
            return refreshedToken;
        }

        public virtual ServerAccessGrant TryGetToken(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var accessToken = MessageUtility.ParseAccessToken(context.Request);
            if (!String.IsNullOrEmpty(accessToken))
                return GetServerAccessGrant(accessToken);
            return null;
        }

        public virtual ServerAccessGrant GetServerAccessGrant(string accessToken)
        {
            return this.serverAccessGrantCache.GetItem(accessToken);
        }

        /// <summary>
        /// 创建服务端授权对象
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>服务端授权对象</returns>
        public ServerAccessGrant CreateServerAccessGrant(int clientId, int userId = 0)
        {
            ServerAccessGrant accessGrant = new ServerAccessGrant(clientId, userId);
            serverAccessRepository.Create(accessGrant);
            return accessGrant;
        }

        public void CreateServerAccessGrant(ServerAccessGrant accessGrant)
        {
            serverAccessRepository.Create(accessGrant);
        }

        private ServerAccessGrant GetServerAccessGrantInner(string accessToken)
        {
            var spec = serverAccessRepository.CreateSpecification()
                .Where(o => o.AccessToken == accessToken);
            return serverAccessRepository.FindOne(spec);
        }

        private ServerAccessGrant GetServerAccessGrantByRefreshToken(string refreshToken)
        {
            var spec = serverAccessRepository.CreateSpecification()
                .Where(o => o.RefreshToken == refreshToken);
            return serverAccessRepository.FindOne(spec);
        }

        private UserProxy GetUserProxy(string userName)
        {
            return userProxRepository.GetUserProxy(userName);
        }

        private ClientAuth GetClientAuth(int clientId)
        {
            var spec = clientRepository.CreateSpecification().Where(o => o.ClientId == clientId);
            return clientRepository.FindOne(spec);
        }

        [DebuggerStepThrough]
        private void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }
}
