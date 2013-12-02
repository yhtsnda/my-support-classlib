using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 用于MVC环境下的授权验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class OAuthAuthorizeAttribute : AuthorizeAttribute
    {
        AspnetCache cache = new AspnetCache() { CacheName = "OAuth" };

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var accessToken = TryParseAccessToken(filterContext.HttpContext.Request);
            if (String.IsNullOrEmpty(accessToken))
                throw new OAuthException("ticket invalid. ticket required");

            var accessGrant = GetAccessGrant(accessToken);
            if (accessGrant == null)
                throw new OAuthException("ticket invalid. ticket expired or invalid") { Code = 401000 };

            AppendRequestData(filterContext.RequestContext, "userId", accessGrant.UserId);
            AppendRequestData(filterContext.RequestContext, "appId", accessGrant.ClientId);
        }

        string TryParseAccessToken(HttpRequestBase request)
        {
            var accessToken = request[Protocal.access_token];

            //兼容之前的接口
            if (String.IsNullOrWhiteSpace(accessToken))
                accessToken = request[Protocal.accesstoken];

            return accessToken;
        }

        AccessGrant GetAccessGrant(string accessToken)
        {
            if (String.IsNullOrEmpty(accessToken))
                return null;

            var accessGrant = cache.Get<AccessGrant>(accessToken);
            if (accessGrant == null)
            {
                accessGrant = OAuthImpl.GetAccessGrant(accessToken);
                if (accessGrant != null)
                {
                    cache.Set(accessToken, accessGrant);
                }
            }
            //判断是否过期
            if (accessGrant != null && accessGrant.IsExpire())
            {
                cache.Remove(typeof(AccessGrant), accessToken);
                accessGrant = null;
            }
            return accessGrant;
        }

        void AppendRequestData(RequestContext context, string name, object value)
        {
            var request = context.HttpContext.Request;
            if (String.IsNullOrEmpty(request.QueryString[name]))
            {
                if (String.IsNullOrEmpty(request.Form[name]))
                {
                    if (!context.RouteData.Values.ContainsKey(name))
                        context.RouteData.Values.Add(name, value);
                }
            }
        }
    }
}
