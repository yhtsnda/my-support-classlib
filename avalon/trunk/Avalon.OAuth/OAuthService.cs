using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Avalon.Framework;

namespace Avalon.OAuth
{
    public class OAuthService : IService
    {
        internal const int TokenExpireCode = 401408;

        IClientAuthorizationRepository clientRepository;
        IAccessGrantRepository tokenRepository;
        IUserProxyRepository userRepository;
        IAuthorizationCodeRepository codeRepository;
        CacheDomain<AccessGrant, string> accessGrantCache;

        public OAuthService(IClientAuthorizationRepository clientRepository, IAccessGrantRepository tokenRepository, IAuthorizationCodeRepository codeRepository, IUserProxyRepository userRepository)
        {
            this.clientRepository = clientRepository;
            this.tokenRepository = tokenRepository;
            this.codeRepository = codeRepository;
            this.userRepository = userRepository;

            accessGrantCache = CacheDomain.CreateSingleKey<AccessGrant, string>(
                o => o.AccessToken,
                GetAccessGrantInner,
                null, 
                CacheConsts.AccessGrantCacheName,
                CacheConsts.AccessGrantCacheFormat
                );
        }

        /// <summary>
        /// 返回授权请求对象
        /// </summary>
        public AuthorizeCodeRequest Authorize(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var codeRequest = MessageUtil.ParseAuthorizeCodeRequest(context.Request);
            var client = GetClientAuthorization(codeRequest.ClientId);

            if (client == null)
                OAuthError(AccessTokenRequestErrorCodes.InvoidClient, "client id invalid.");

            if (client.Status != ClientAuthorizeStatus.Normal)
                OAuthError(AccessTokenRequestErrorCodes.UnauthorizedClient, "client unauthorized", 401);

            client.ValidRedirectUri(codeRequest.RedirectUri);
            return codeRequest;
        }

        /// <summary>
        /// 处理OAuth接口的请求，并返回响应对象
        /// </summary>
        /// <remarks>
        /// 包含 token 的接口请求及 authorize code 的接口请求。
        /// </remarks>
        public object Process(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            ResponseType responseType = ResponseType.AccessToken;
            var responseTypeString = MessageUtil.TryGetString(context.Request, Protocal.response_type);
            if (!String.IsNullOrEmpty(responseTypeString))
            {
                if (!ResponseTypeExtend.TryParse(responseTypeString, out responseType))
                    throw new OAuthException(AccessTokenRequestErrorCodes.InvalidRequest, "wrong response_type value", 400);
            }

            var request = context.Request;
            switch (responseType)
            {
                case ResponseType.AccessToken:
                    var tokenRequest = MessageUtil.ParseTokenRequest(request);
                    tokenRequest.OAuthService = this;
                    return tokenRequest.Token();

                case ResponseType.AuthorizationCode:
                    var authorizeRequest = MessageUtil.ParseAuthorizeRequest(request);
                    authorizeRequest.OAuthService = this;
                    return authorizeRequest.Authorize();
            }
            return null;
        }

        public virtual AccessGrant TryGetToken(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var accessToken = MessageUtil.TryParseAccessToken(context.Request);
            if (!String.IsNullOrEmpty(accessToken))
                return GetAccessGrant(accessToken);

            return null;
        }

        public virtual AccessGrant TokenValid(HttpContextBase context)
        {
            Arguments.NotNull(context, "context");

            var accessToken = MessageUtil.ParseAccessToken(context.Request);
            return TokenValid(accessToken);
        }

        public virtual AccessGrant TokenValid(string accessToken)
        {
            Arguments.NotNull(accessToken, "accessToken");

            var accessGrant = GetAccessGrant(accessToken);

            if (accessGrant == null)
                OAuthError(BearerTokenErrorCodes.InvalidToken, "access token invalid", 401000);

            if (accessGrant.IsExpire())
                OAuthError(BearerTokenErrorCodes.InvalidToken, "access token expired", TokenExpireCode);

            return accessGrant;
        }

        #region AccessGrant

        public AccessGrant GetAccessGrant(string accessToken)
        {
            return accessGrantCache.GetItem(accessToken);
        }

        public AccessGrant GetAccessGrantByRefreshToken(string refreshToken)
        {
            var spec = tokenRepository.CreateSpecification().Where(o => o.RefreshToken == refreshToken);
            return tokenRepository.FindOne(spec);
        }

        public AccessGrant CreateAccessGrant(int appId, int appCode = 0, long userId = 0)
        {
            AccessGrant accessGrant = new AccessGrant(appId, appCode, userId);
            tokenRepository.Create(accessGrant);
            return accessGrant;
        }

        public void CreateAccessGrant(AccessGrant accessGrant)
        {
            tokenRepository.Create(accessGrant);
        }

        public void DeleteAccessGrant(AccessGrant accessGrant)
        {
            tokenRepository.Delete(accessGrant);
        }

        AccessGrant GetAccessGrantInner(string accessToken)
        {
            var spec = tokenRepository.CreateSpecification().Where(o => o.AccessToken == accessToken);
            return tokenRepository.FindOne(spec);
        }
        #endregion

        #region AuthorizationCode

        public AuthorizationCode GetAuthorizationCode(string code)
        {
            return codeRepository.Get(code);
        }

        public AuthorizationCode CreateAuthorizationCode(int clientId, long userId)
        {
            Arguments.That(clientId > 0, "clientId", "必须大于0");
            Arguments.That(userId > 0, "userId", "必须大于0");

            AuthorizationCode code = new AuthorizationCode(clientId, userId);
            codeRepository.Create(code);
            return code;
        }

        public void DeleteAuthorizationCode(AuthorizationCode code)
        {
            codeRepository.Delete(code);
        }
        #endregion

        public ValidResult ValidPassword(string userName, string password, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            return userRepository.ValidPassword(userName, password, platCode, browser, ipAddress, extendFiled);
        }

        public ValidResult ValidThirdToken(string accessToken, int mappingType, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            return userRepository.ValidThirdToken(accessToken, mappingType, platCode, browser, ipAddress, extendFiled);
        }

        public ValidResult ValidPassport91(long password91Id, string userName, string password, string cookieOrdernumberMaster, string cookieOrdernumberSlave, string cookieCheckcode, string cookieSiteflag, long platCode, string browser = null, string ipAddress = null, string extendFiled = null)
        {
            return userRepository.ValidPassport91(password91Id, userName, password, cookieOrdernumberMaster, cookieOrdernumberSlave, cookieCheckcode, cookieSiteflag, platCode, browser, ipAddress, extendFiled);
        }

        public ClientAuthorization GetClientAuthorization(int clientId)
        {
            return clientRepository.Get(clientId);
        }

        public IList<ClientAuthorization> GetAllClientAuthorizationList()
        {
            var spec = clientRepository.CreateSpecification();
            return clientRepository.FindAll(spec);
        }

        public ClientAuthorization CreateClientAuthorization(string name, string description = null, string remark = null, string redirectPath = null)
        {
            var guid = Guid.NewGuid().ToString("N");
            ClientAuthorization entity = new ClientAuthorization()
            {
                Name = name,
                Description = description,
                Secret = guid,
                Status = ClientAuthorizeStatus.Normal,
                CreateTime = NetworkTime.Now,
                UpdateTime = NetworkTime.Now,
                Remark = remark,
                RedirectPath = redirectPath
            };
            clientRepository.Create(entity);
            return entity;
        }

        public PagingResult<ClientAuthorization> GetClientAuthorizationList(ClientAuthorizationFilter filter)
        {
            return clientRepository.GetClientAuthorizationList(filter);
        }

        [DebuggerStepThrough]
        void OAuthError(string errorCode, string message, int code = 400)
        {
            throw new OAuthException(errorCode, message, code);
        }
    }
}
