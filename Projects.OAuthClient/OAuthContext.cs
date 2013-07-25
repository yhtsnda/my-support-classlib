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
        AccessGrant userAccessGrant;
        AccessGrant appAccessGrant;

        /// <summary>
        /// 获取当前的上下文对象
        /// </summary>
        public static OAuthContext Current
        {
            get { return OAuthService.CurrentOAuthContext; }
        }

        internal OAuthContext(AccessGrant appAccessGrant, AccessGrant userAccessGrant)
        {
            this.appAccessGrant = appAccessGrant;
            this.userAccessGrant = userAccessGrant;
        }

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool IsAppAuthorized
        {
            get { return appAccessGrant != null; }
        }

        public bool IsUserAuthorized
        {
            get { return userAccessGrant != null; }
        }

        public string AppAccessToken
        {
            get
            {
                var oauthScope = OAuthScope.PeekOAuthScope();
                if (oauthScope != null)
                    return oauthScope.AccessToken;

                EnsureAppAuthorize();
                return appAccessGrant.AccessToken;
            }
        }

        public string UserAccessToken
        {
            get
            {
                var oauthScope = OAuthScope.PeekOAuthScope();
                if (oauthScope != null)
                    return oauthScope.AccessToken;

                EnsureUserAuthorize();
                return userAccessGrant.AccessToken;
            }
        }

        internal AccessGrant UserAccessGrant
        {
            get { return userAccessGrant; }
        }

        internal AccessGrant AppAccessGrant
        {
            get { return appAccessGrant; }
        }

        void EnsureAppAuthorize()
        {
            if (appAccessGrant == null)
                throw new OAuthException("尚未授权。请先使用 OAuthContext.AppAuthorize 进行授权。");
        }


        void EnsureUserAuthorize()
        {
            if (userAccessGrant == null)
                throw new OAuthException("尚未授权。请先使用 OAuthContext.UserAuthorize 进行授权。");
        }
    }
}
