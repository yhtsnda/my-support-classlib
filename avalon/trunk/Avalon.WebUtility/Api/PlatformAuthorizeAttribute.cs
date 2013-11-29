using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PlatformAuthorizeAttribute : FilterAttribute, IAuthorizationFilter, IActionFilter
    {
        /// <summary>
        /// 在过程请求授权时调用
        /// </summary>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContent");

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // If a child action cache block is active, we need to fail immediately, even if authorization
                // would have succeeded. The reason is that there's no way to hook a callback to rerun
                // authorization before the fragment is served from the cache, so we can't guarantee that this
                // filter will be re-run on subsequent requests.
                throw new InvalidOperationException("Can not use with in child action cache.");
            }

            if (IsSkipAuthorization(filterContext.ActionDescriptor))
                return;

            if (AuthorizeCore(filterContext.HttpContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.

                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(new HttpCacheValidateHandler(CacheValidateHandler), null);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        /// 确定是否获得访问核心框架的授权。
        /// </summary>
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            IPrincipal user = httpContext.User;
            return user.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 处理授权失败的 HTTP 请求
        /// </summary>
        void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = HandleUnauthorizedRequest(filterContext, filterContext.ActionDescriptor);
        }

        /// <summary>
        /// 处理授权失败的 HTTP 请求
        /// </summary>
        void HandleUnauthorizedRequest(ActionExecutingContext filterContext)
        {
            filterContext.Result = HandleUnauthorizedRequest(filterContext, filterContext.ActionDescriptor);
        }

        protected virtual ActionResult HandleUnauthorizedRequest(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            bool hasApiAttr = actionDescriptor.IsDefined(typeof(ApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(ApiAttribute), true);

            // 忽略 ActionResult 的返回类型
            if (actionDescriptor is ReflectedActionDescriptor)
            {
                var reflectedActionDescriptor = (ReflectedActionDescriptor)actionDescriptor;
                var returnType = reflectedActionDescriptor.MethodInfo.ReturnType;
                if (returnType == typeof(ActionResult) || returnType.IsSubclassOf(typeof(ActionResult)))
                    hasApiAttr = false;
            }

            // 302 
            if (hasApiAttr)
                return new PlatformApiUnauthorizedResult();

            var request = controllerContext.HttpContext.Request;
            var url = request.HttpMethod.Equals("GET", StringComparison.CurrentCultureIgnoreCase) ? request.Url.ToString() : null;
            return new PlatformHttpLoginRedirectResult(url);
        }

        /// <summary>
        /// 在缓存模块请求授权时调用。
        /// </summary>
        HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!GlobalConfig.AuthEnable)
            //    return;

            if (IsSkipAuthorization(filterContext.ActionDescriptor))
                return;

            long userId;
            if (long.TryParse(filterContext.HttpContext.User.Identity.Name, out userId))
            {
                if (OnAuthorizeUser(filterContext, userId))
                    return;
            }
            HandleUnauthorizedRequest(filterContext);
        }

        bool IsSkipAuthorization(ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                                     || actionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }

        protected virtual bool OnAuthorizeUser(ActionExecutingContext filterContext, long userId)
        {
            return true;
        }
    }
}