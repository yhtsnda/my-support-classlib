using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    public class NotModifiedFilterAttribute : ActionFilterAttribute
    {
        int expireSeconds;

        public NotModifiedFilterAttribute(int expireSeconds)
        {
            this.expireSeconds = expireSeconds;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if ((IsSourceModified(request) == false))
            {
                filterContext.Result = new HttpStatusCodeResult(304, "Not Modified");
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);
            var span = TimeSpan.FromSeconds(expireSeconds);
            var now = DateTime.Now;
            response.Cache.SetMaxAge(span);
            response.Cache.SetLastModified(now);
            response.Cache.SetExpires(now.Add(span));

            base.OnActionExecuted(filterContext);
        }

        private bool IsSourceModified(HttpRequestBase request)
        {
            string requestIfModifiedSinceHeader = request.Headers["If-Modified-Since"] ?? string.Empty;

            DateTime requestIfModifiedSince;
            DateTime.TryParse(requestIfModifiedSinceHeader, out requestIfModifiedSince);

            return requestIfModifiedSince == DateTime.MinValue || requestIfModifiedSince.AddSeconds(expireSeconds) < DateTime.Now;
        }
    }
}
