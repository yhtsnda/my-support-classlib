using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool;
using Projects.Tool.Http;
using Projects.Tool.Util;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 提供 OAuth 服务（自动续约）
    /// </summary>
    public abstract class AbstractOAuthProvider
    {
        AccessGrant appAccessGrant;

        /// <summary>
        /// 获取授权
        /// </summary>
        public virtual OpenApiResult<AccessGrant> UserAuthorize(SimpleTokenData tokenData)
        {
            var result = OAuthOperator.GetToken(tokenData);
            if (result.Code == 0)
            {
                OnUserAuthorizeSuccess(result.Data);
            }
            return result;
        }

        public virtual AccessGrant AppAuthorize()
        {
            var tokenData = new ClientCredentialsTokenData(OAuthContext.ClientId, OAuthContext.ClientSecret);
            var result = OAuthOperator.GetToken(tokenData);
            if (result.Code != 0)
                throw new OAuthException("应用授权失败。" + result.Message);

            OnAppAuthorizeSuccess(result.Data);
            return result.Data;
        }

        public virtual long GetPlatCode()
        {
            return OAuthContext.PlatCode;
        }

        /// <summary>
        /// 当成功授权时处理
        /// </summary>
        protected abstract void OnUserAuthorizeSuccess(AccessGrant accessGrant);

        protected virtual void OnAppAuthorizeSuccess(AccessGrant accessGrant)
        {
            appAccessGrant = accessGrant;
        }

        /// <summary>
        /// 获取当前的 OAuth 上下文对象
        /// </summary>
        public virtual OAuthContext GetCurrent()
        {
            var appAccessGrant = GetAppAccessGrant();
            var userAccessGrant = GetCurrentUserAccessGrant();

            EnsureAccessGrantValid(ref appAccessGrant);
            EnsureAccessGrantValid(ref userAccessGrant);

            return new OAuthContext(appAccessGrant, userAccessGrant);
        }

        /// <summary>
        /// 获取当前的 Access Grant
        /// </summary>
        protected abstract AccessGrant GetCurrentUserAccessGrant();

        protected virtual AccessGrant GetAppAccessGrant()
        {
            return appAccessGrant;
        }

        /// <summary>
        /// 刷新 AccessToken
        /// </summary>
        public virtual AccessGrant RefreshToken(AccessGrant accessGrant)
        {
            RefreshTokenData tokenData = CreateRefreshTokenTokenData(accessGrant);
            var result = OAuthOperator.GetToken(tokenData);
            if (result.Code == 0)
            {
                var ag = result.Data;
                if (ag.UserId == 0)
                    OnAppAuthorizeSuccess(ag);
                else
                    OnUserAuthorizeSuccess(ag);
                return ag;
            }

            throw new OAuthException(result.Code + ":" + result.Message);
        }

        /// <summary>
        /// 判断是否应该刷新 AccessToken
        /// </summary>
        protected virtual bool ShouldRefreshToken(AccessGrant acessGrant)
        {
            return !acessGrant.IsEffective() ||
                (NetworkTime.Now - acessGrant.CreateTime).TotalSeconds / (acessGrant.ExpireTime - acessGrant.CreateTime).TotalSeconds > 0.75;
        }

        /// <summary>
        /// 创建 RefreshToken 的请求对象
        /// </summary>
        protected virtual RefreshTokenData CreateRefreshTokenTokenData(AccessGrant accessGrant)
        {
            return new RefreshTokenData(OAuthContext.ClientId, OAuthContext.ClientSecret, accessGrant.RefreshToken);
        }

        /// <summary>
        /// 当使用 OAuthHttpClient 发出 OpenApi 接口的调用时
        /// </summary>
        public abstract Uri OnOpenApiRequest(Uri uri);

        void EnsureAccessGrantValid(ref AccessGrant accessGrant)
        {
            if (accessGrant != null && ShouldRefreshToken(accessGrant))
                accessGrant = RefreshToken(accessGrant);
        }

        protected Uri GetUri(Uri uri, string accessToken)
        {
            return GetUri(uri, Protocal.ACCESS_TOKEN, accessToken);
        }

        protected Uri GetUri(Uri uri, string key, string value)
        {
            UriPathBuilder builder = new UriPathBuilder(uri.AbsoluteUri);
            var url = builder.Append(key, value)
                .ToString();
            return new Uri(url);
        }
    }
}
