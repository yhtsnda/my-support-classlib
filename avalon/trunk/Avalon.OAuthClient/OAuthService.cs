using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avalon.OAuthClient
{
    public class OAuthService
    {
        static ILog log = LogManager.GetLogger("OAuth");

        static bool inited = false;

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
        public static void Init()
        {
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

                OAuthProvider = (AbstractOAuthProvider)Avalon.Utility.FastActivator.Create(providerType);
                OAuthProvider.OnInit();
            }
        }


        public static OpenApiResult<AccessGrant> Authorize(TokenData tokenData)
        {
            EnsureInit();

            return OAuthProvider.Authorize(tokenData);
        }

        public static OpenApiResult<AuthorizationCode> GetAuthorizationCode(AuthorizeData authorizeData)
        {
            EnsureInit();

            return OAuthImpl.GetCode(authorizeData);
        }

        public static AccessGrant Valid(string accessToken)
        {
            return OAuthImpl.GetAccessGrant(accessToken);
        }

        static void EnsureInit()
        {
            if (!inited)
                throw new OAuthException("尚未初始化。请先使用 OAuthContext.Init 进行初始化。");
        }
    }
}
