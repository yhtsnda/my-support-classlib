using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 信任客户端模型
    /// </summary>
    public class CredentialsClientOAuthProvider : AbstractOAuthProvider
    {
        Dictionary<int, AccessGrant> appAccessGrant;
        object syncRoot = new object();

        public CredentialsClientOAuthProvider()
        {
            AppMetadata = new AppMetadata();
            appAccessGrant = new Dictionary<int, AccessGrant>();
        }

        public virtual AppMetadata AppMetadata
        {
            get;
            set;
        }

        public override Uri OnOpenApiRequest(Uri uri)
        {
            string accessToken = GetCurrent().AccessToken;
            return GetUri(uri, accessToken);
        }

        protected void EnsureAppMetadata()
        {
            if (AppMetadata == null || AppMetadata.ClientId == 0 || String.IsNullOrEmpty(AppMetadata.ClientSecret))
                throw new ConfigurationErrorsException("缺少配置项 OAuth.CredentialsClient.ClientId、OAuth.CredentialsClient.ClientSecret 或者 AppMetadata 中 ClientId、ClientSecret 未赋值。");
        }

        protected override AccessGrant GetCurrentAccessGrant()
        {
            EnsureAppMetadata();
            var am = AppMetadata;
            AccessGrant accessGrant;
            if (!appAccessGrant.TryGetValue(am.ClientId,out accessGrant))
            {
                lock (syncRoot)
                {
                    if (!appAccessGrant.TryGetValue(am.ClientId, out accessGrant))
                    {
                        var token = new ClientCredentialsTokenData(am.ClientId, am.ClientSecret);
                        accessGrant = AuthorizeAndValid(token);
                        appAccessGrant.Add(accessGrant.ClientId, accessGrant);
                    }
                }
            }
            return accessGrant;
        }

        protected internal override void OnInit()
        {
            if (AppMetadata.ClientId == 0)
            {
                var clientId = ConfigurationManager.AppSettings["OAuth.CredentialsClient.ClientId"];
                if (!String.IsNullOrEmpty(clientId))
                    AppMetadata.ClientId = Int32.Parse(clientId);

                var clientSecret = ConfigurationManager.AppSettings["OAuth.CredentialsClient.ClientSecret"];
                if (!String.IsNullOrEmpty(clientSecret))
                    AppMetadata.ClientSecret = clientSecret;
            }
        }

        protected override RefreshTokenTokenData CreateRefreshTokenTokenData(AccessGrant accessGrant)
        {
            EnsureAppMetadata();
            var am = AppMetadata;
            if (accessGrant.ClientId != am.ClientId)
                throw new ArgumentException(String.Format("给定的 AccessToken 的 AppId {0} 与系统的 {1} 不一致。", accessGrant.ClientId, am.ClientId));
            return new RefreshTokenTokenData(accessGrant.ClientId, am.ClientSecret, accessGrant.RefreshToken);
        }
    }


}
