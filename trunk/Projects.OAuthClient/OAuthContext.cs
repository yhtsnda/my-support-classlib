using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Projects.Tool.Util;
using Projects.Tool.Reflection;

namespace Projects.OAuthClient
{
    /// <summary>
    /// OAuth请求上下文
    /// </summary>
    public sealed class OAuthContext
    {
        private static OAuthContext innerContext = null;

        private static bool inited = false;
        private AccessGrant userAccessGrant;
        private AccessGrant appAccessGrant;

        protected OAuthContext()
        {
        }

        ///<summary>
        /// OAuth授权上下文
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="clientSecret">客户端秘钥</param>
        /// <param name="platCode">平台编码</param>
        protected OAuthContext(int clientId, string clientSecret)
        {
            var strPlatCode = ConfigurationManager.AppSettings["OAuth.PlatCode"];
            if(String.IsNullOrWhiteSpace(strPlatCode))
                throw new ConfigurationErrorsException("缺少 OAuth.PlatCode 配置项");
            if(strPlatCode.Length != 12)
                throw new ConfigurationErrorsException("OAuth.PlatCode 配置项必须为12位");
            long platCode = 0L;
            if(!Int64.TryParse(strPlatCode, out platCode))
                throw new ConfigurationErrorsException("OAuth.PlatCode 配置项必须为数字");

            if (String.IsNullOrEmpty(OAuthServicePath))
            {
                var servicePath = ConfigurationManager.AppSettings["OAuth.ServicePath"];
                if (String.IsNullOrWhiteSpace(servicePath))
                    throw new ConfigurationErrorsException("缺少 OAuth.ServicePath 配置项");
                OAuthServicePath = servicePath;
            }

            if (OAuthProvider == null)
            {
                var providerTypeString = ConfigurationManager.AppSettings["OAuth.Provider"];
                if (String.IsNullOrWhiteSpace(providerTypeString))
                    throw new ConfigurationErrorsException("缺少 OAuth.Provider 配置项");
                var providerType = Type.GetType(providerTypeString);
                if (providerType == null)
                    throw new ConfigurationErrorsException("无法创建类型 " + providerTypeString + " 的实例");

                OAuthProvider = (AbstractOAuthProvider)FastActivator.Create(providerType);
            }

            ClientId = clientId;
            ClientSecret = clientSecret;
            PlatCode = platCode;
            inited = true;
        }

        /// <summary>
        /// OAuth授权上下文
        /// </summary>
        /// <param name="appAccessGrant">应用授权</param>
        /// <param name="userAccessGrant">个人授权</param>
        internal OAuthContext(AccessGrant appAccessGrant, AccessGrant userAccessGrant)
        {
            this.appAccessGrant = appAccessGrant;
            this.userAccessGrant = userAccessGrant;
        }

        /// <summary>
        /// 获取当前的上下文对象
        /// </summary>
        public static OAuthContext Current
        {
            get
            {
                CheckInit();
                return OAuthProvider.GetCurrent();
            }
        }

        /// <summary>
        /// 获取 OAuth 服务的地址
        /// </summary>
        public static string OAuthServicePath { get; private set; }

        /// <summary>
        /// 获取 OAuth 服务的提供对象
        /// </summary>
        public static AbstractOAuthProvider OAuthProvider { get; private set; }

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

        /// <summary>
        /// 应用授权凭据
        /// </summary>
        public string AppAccessToken
        {
            get
            {
                var oauthScope = OAuthScope.Peek();
                if (oauthScope != null)
                    return oauthScope.AccessToken;

                CheckAppAuthorize();
                return appAccessGrant.AccessToken;
            }
        }

        /// <summary>
        /// 用户授权凭据
        /// </summary>
        public string UserAccessToken
        {
            get
            {
                var oauthScope = OAuthScope.Peek();
                if(oauthScope != null)
                    return oauthScope.AccessToken;

                CheckUserAuthorize();
                return userAccessGrant.AccessToken;
            }
        }

        /// <summary>
        /// 准备OAuth认证(只需在应用启动时调用一次)
        /// </summary>
        public static void GetReady(int clientId, string clientSecret)
        {
            if(innerContext != null)
                throw new OAuthException("OAuth 授权已初始化,无需重复初始化");

            innerContext = new OAuthContext(clientId, clientSecret);
        }

        /// <summary>
        /// 应用授权
        /// </summary>
        /// <returns>授权信息</returns>
        public AccessGrant AppAuthorize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <returns>授权信息</returns>
        public AccessGrant UserAuthorize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 使用MD5获取的加密后的密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            string appkey = "fdjf,jkgfkl"; //加一特殊的字符后再加密，这样更安全些

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] a = Encoding.Default.GetBytes(appkey);
            byte[] datSource = Encoding.Default.GetBytes(password);
            byte[] b = new byte[a.Length + 4 + datSource.Length];

            int i;
            for (i = 0; i < datSource.Length; i++)
            {
                b[i] = datSource[i];
            }

            b[i++] = 163;
            b[i++] = 172;
            b[i++] = 161;
            b[i++] = 163;

            for (int k = 0; k < a.Length; k++)
            {
                b[i] = a[k];
                i++;
            }

            byte[] newSource = MD5.ComputeHash(b);
            return StringUtil.ToHex(newSource);
        }

        private static void CheckInit()
        {
            if(!inited)
                throw new OAuthException("OAuth授权服务尚未初始化,请先使用OAuthContext.Init进行初始化");
        }

        private void CheckAppAuthorize()
        {
            if (appAccessGrant == null)
                throw new OAuthException("应用未授权,请通过OAuthContext.AppAuthorize方法进行授权");
        }

        private void CheckUserAuthorize()
        {
            if(userAccessGrant == null)
                throw new OAuthException("应用未授权,请通过OAuthContext.UserAuthorize方法进行授权");
        }
    }
}
