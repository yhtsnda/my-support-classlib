
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using Avalon.Utility;
using System.Security.Cryptography;
using System.Web;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 授权的上下文对象
    /// </summary>
    public class OAuthContext
    {
        AccessGrant accessGrant;

        /// <summary>
        /// 获取当前的上下文对象
        /// </summary>
        public static OAuthContext Current
        {
            get { return OAuthService.CurrentOAuthContext; }
        }

        internal OAuthContext(AccessGrant accessGrant)
        {
            this.accessGrant = accessGrant;
        }

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool IsAuthorized
        {
            get { return accessGrant != null; }
        }

        public string AccessToken
        {
            get
            {
                //var oauthScope = OAuthScope.PeekOAuthScope();
                //if (oauthScope != null)
                //    return oauthScope.AccessToken;

                EnsureAuthorize();
                return accessGrant.AccessToken;
            }
        }

        public AccessGrant AccessGrant
        {
            get { return accessGrant; }
        }

        void EnsureAuthorize()
        {
            if (!IsAuthorized)
                throw new OAuthException("尚未授权。请先使用 OAuthContext.Authorize 进行授权。");
        }
    }
}
