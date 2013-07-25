using Projects.Tool.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Projects.OAuthClient
{
    public class OAuthService
    {
        private static ILog log = LogManager.GetLogger("OAuth");
        private static bool inited = false;

        /// <summary>
        /// 获取 OAuth 服务的地址
        /// </summary>
        public static string OAuthServicePath { get; set; }

        /// <summary>
        /// 获取 OAuth 服务的主机名称
        /// </summary>
        public static string OAuthServiceHost { get; set; }

        /// <summary>
        /// 获取 OAuth 服务的提供对象
        /// </summary>
        public static AbstractOAuthProvider OAuthProvider { get; set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public static int ClientId { get; private set; }

        /// <summary>
        /// 客户端标识
        /// </summary>
        public static string ClientSecret { get; private set; }

        /// <summary>
        /// 平台标识
        /// </summary>
        public static long PlatCode { get; private set; }

        internal static OAuthContext CurrentOAuthContext
        {
            get
            {
                EnsureInit();
                return OAuthProvider.GetCurrent();
            }
        }

        /// <summary>
        /// OAuth 服务的初始化
        /// </summary>
        public static void Init(int clientId, string clientSecret, long platCode = 0)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            PlatCode = platCode;
            inited = true;

            if (String.IsNullOrEmpty(OAuthServicePath))
            {
                var servicePath = ConfigurationManager.AppSettings["OAuth.ServicePath"];
                if (String.IsNullOrWhiteSpace(servicePath))
                    throw new ConfigurationErrorsException("缺少 OAuth.ServicePath 配置项。");
                OAuthServicePath = servicePath;
            }

            if (String.IsNullOrEmpty(OAuthServiceHost))
            {
                var serviceHost = ConfigurationManager.AppSettings["OAuth.ServiceHost"];
                if (!String.IsNullOrWhiteSpace(serviceHost))
                {
                    OAuthServiceHost = serviceHost;
                }
            }

            if (OAuthProvider == null)
            {
                var providerTypeString = ConfigurationManager.AppSettings["OAuth.Provider"];
                if (String.IsNullOrWhiteSpace(providerTypeString))
                    throw new ConfigurationErrorsException("缺少 OAuth.Provider 配置项。");
                var providerType = Type.GetType(providerTypeString);
                if (providerType == null)
                    throw new ConfigurationErrorsException("无法创建类型 " + providerTypeString + " 的实例。");

                OAuthProvider = (AbstractOAuthProvider)Projects.Tool.Reflection.FastActivator.Create(providerType);
            }
        }

        /// <summary>
        /// 应用授权
        /// </summary>
        /// <returns></returns>
        public static AccessGrant AppAuthorize()
        {
            EnsureInit();
            log.Info("app authorize begin");
            AccessGrant accessGrant = OAuthProvider.AppAuthorize();
            log.InfoFormat("app authorize success. access token: {0}", ag.AccessToken);
            return accessGrant;
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <returns></returns>
        public static OpenApiResult<AccessGrant> UserAuthorize(SimpleTokenData tokenData)
        {
            EnsureInit();
            return OAuthProvider.UserAuthorize(tokenData);
        }

        /// <summary>
        /// 获取授权验证码
        /// </summary>
        public static OpenApiResult<AuthorizationCode> GetAuthorizationCode(AuthorizeData authorizeData)
        {
            EnsureInit();
            return OAuthOperator.GetCode(authorizeData);
        }

        /// <summary>
        /// 验证AccessToken
        /// </summary>
        public static AccessGrant Valid(string accessToken)
        {
            return OAuthOperator.GetAccessGent(accessToken);
        }

        /// <summary>
        /// 用户密码加密
        /// </summary>
        public static string EncryptPassword(string password)
        {
            return Projects.Tool.Util.StringUtil.EncryptPassword(password);
        }

        private static void EnsureInit()
        {
            if (!inited)
                throw new OAuthException("尚未初始化.请先使用 Init 进行初始化");
        }
    }
}
