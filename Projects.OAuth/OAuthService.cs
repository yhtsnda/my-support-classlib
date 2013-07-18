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
        internal const int TOKEN_EXPIRE_CODE = 401408;

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

        public AuthCodeRequest Authorize(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var codeRequest = MessageUtility.ParseAuthCodeRequest(context.Request);
            var client = GetClientAuth(codeRequest.ClientId);

            if (client == null)
                OAuthError(AccessTokenRequestErrorCode.InvoidClient, "client id invalid.");
            if(client.Status != ClientAuthStatus.Enabled)
                OAuthError(AccessTokenRequestErrorCode.UnauthorizedClient, "client unauthorized", 401);

            client.ValidateCallbackUri(codeRequest.RedirectUri);
            return codeRequest;
        }

        public object Process(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            AuthResponseType responseType = AuthResponseType.AccessToken;
            var responseTypeString = MessageUtility.TryGetString(context.Request, Protocal.RESPONSE_TYPE);
            if (!String.IsNullOrEmpty(responseTypeString))
            {
                if (!AuthResponseTypeExtend.TryParse(responseTypeString, out responseType))
                    throw new OAuthException(AccessTokenRequestErrorCode.InvalidRequest, "wrong response_type value", 400);
            }

            var request = context.Request;
            switch (responseType)
            {
                case AuthResponseType.AccessToken:
                    var tokenRequest = MessageUtility.ParseTokenRequest(request);
                    tokenRequest.OAuthService = this;
                    return tokenRequest.Token();

                case AuthResponseType.AuthorizationCode:
                    var authorizeRequest = MessageUtility.ParseAuthRequest(request);
                    authorizeRequest.OAuthService = this;
                    return authorizeRequest.Authorize();
            }
            return null;
        }

        public ServerAccessGrant TryGetToken(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var accessToken = MessageUtility.ParseAccessToken(context.Request);
            if (!String.IsNullOrEmpty(accessToken))
                return GetServerAccessGrant(accessToken);
            return null;
        }

        public virtual ServerAccessGrant TokenValid(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");
            var accessToken = MessageUtility.ParseAccessToken(context.Request);
            return TokenValid(accessToken);
        }

        public virtual ServerAccessGrant TokenValid(string accessToken)
        {
            Arguments.NotNull(accessToken, "accessToken");
            var accessGrant = GetServerAccessGrant(accessToken);

            if(accessGrant == null)
                OAuthError(BearerTokenErrorCode.InvalidToken, "access token invalid", 401000);
            if(!accessGrant.IsEffective())
                OAuthError(BearerTokenErrorCode.InvalidToken, "access token expired", TOKEN_EXPIRE_CODE);

            return accessGrant;
        }

        public ServerAccessGrant GetServerAccessGrant(string accessToken)
        {
            return this.serverAccessGrantCache.GetItem(accessToken);
        }

        public ServerAccessGrant GetServerAccessGrantByRefreshToken(string refreshToken)
        {
            var spec = serverAccessRepository.CreateSpecification().Where(o => o.RefreshToken == refreshToken);
            return serverAccessRepository.FindOne(spec);
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

        public void DeleteServerAccessGrant(ServerAccessGrant accessGrant)
        {
            serverAccessRepository.Delete(accessGrant);
        }

        public ValidResult ValidatePassword(string userName, string password, long platCode,
            string browser = null, string ipAddress = null, string extendFiled = null)
        {
            return userProxRepository.ValidPassword(userName, password, platCode, browser, ipAddress, extendFiled);
        }

        public ValidResult ValidateThirdToken(string accessToken, int mappingType, long platCode,
            string browser = null, string ipAddress = null, string extendFiled = null)
        {
            return userProxRepository.ValidThirdToken(accessToken, mappingType, platCode, browser, ipAddress, extendFiled);
        }

        private ServerAccessGrant GetServerAccessGrantInner(string accessToken)
        {
            var spec = serverAccessRepository.CreateSpecification()
                .Where(o => o.AccessToken == accessToken);
            return serverAccessRepository.FindOne(spec);
        }

        public ClientAuth GetClientAuth(int clientId)
        {
            return clientRepository.Get(clientId);
        }

        public AuthorizationCode GetAuthorizationCode(string code)
        {
            return authCodeRepository.Get(code);
        }

        public AuthorizationCode CreateAuthorizationCode(int clientId, int userId)
        {
            Arguments.That(clientId > 0, "clientId", "必须大于0");
            Arguments.That(userId > 0, "userId", "必须大于0");

            AuthorizationCode code = new AuthorizationCode(clientId, userId);
            authCodeRepository.Create(code);
            return code;
        }

        public void DeleteAuthorizationCode(AuthorizationCode code)
        {
            authCodeRepository.Delete(code);
        }

        [DebuggerStepThrough]
        private void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }
}
