using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 提供 OAuth 服务（自动续约）
    /// </summary>
    public abstract class AbstractOAuthProvider
    {
        /// <summary>
        /// 获取当前的 OAuth 上下文对象
        /// </summary>
        public virtual OAuthContext GetCurrent()
        {
            var accessGrant = GetCurrentAccessGrant();
            EnsureAccessGrantValid(ref accessGrant);
            return new OAuthContext(accessGrant);
        }

        /// <summary>
        /// 刷新 AccessToken
        /// </summary>
        public virtual AccessGrant RefreshToken(AccessGrant accessGrant)
        {
            RefreshTokenTokenData tokenData = CreateRefreshTokenTokenData(accessGrant);
            var result = OAuthImpl.GetToken(tokenData);
            if (result.Code == 0)
            {
                var ag = result.Data;
                OnAuthorizeSuccess(ag);
                return ag;
            }
            //当刷新失败时，不抛出异常。2013-8-23 HHB
            return null;
            //
            //throw new OAuthException(result.Code + ":" + result.Message);
        }

        /// <summary>
        /// 当使用 OAuthHttpClient 发出 OpenApi 接口的调用时
        /// </summary>
        public abstract Uri OnOpenApiRequest(Uri uri);

        protected abstract AccessGrant GetCurrentAccessGrant();

        /// <summary>
        /// 创建 RefreshToken 的请求对象
        /// </summary>
        protected abstract RefreshTokenTokenData CreateRefreshTokenTokenData(AccessGrant accessGrant);

        public virtual OpenApiResult<AccessGrant> Authorize(TokenData tokenData)
        {
            var result = OAuthImpl.GetToken(tokenData);
            if (result.Code == 0)
                OnAuthorizeSuccess(result.Data);
            return result;
        }

        protected AccessGrant AuthorizeAndValid(TokenData tokenData)
        {
            var result = Authorize(tokenData);
            if (result.Code != 0)
                throw new ArgumentException("授权错误！");
            return result.Data;
        }

        protected virtual void OnAuthorizeSuccess(AccessGrant accessGrant)
        {
        }

        /// <summary>
        /// 判断是否应该刷新 AccessToken
        /// </summary>
        protected virtual bool ShouldRefreshToken(AccessGrant acessGrant)
        {
            return acessGrant.IsExpire() ||
                (NetworkTime.Now - acessGrant.CreateTime).TotalSeconds / (acessGrant.ExpireTime - acessGrant.CreateTime).TotalSeconds > 0.75;
        }

        void EnsureAccessGrantValid(ref AccessGrant accessGrant)
        {
            if (accessGrant != null && ShouldRefreshToken(accessGrant))
                accessGrant = RefreshToken(accessGrant);
        }

        protected Uri GetUri(Uri uri, string accessToken)
        {
            return GetUri(uri, Protocal.access_token, accessToken);
        }

        protected Uri GetUri(Uri uri, string key, string value)
        {
            var url = UriPath.AppendArguments(uri.AbsoluteUri, key, value);
            return new Uri(url);
        }

        internal protected virtual void OnInit()
        { }
    }
}
