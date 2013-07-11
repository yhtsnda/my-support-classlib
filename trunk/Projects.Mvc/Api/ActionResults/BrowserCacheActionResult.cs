using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Projects.Tool;
using System.Web;
using System.Net;
using System.IO;
using Projects.Tool.Util;
using System.Web.Script.Serialization;
using System.Globalization;

namespace Projects.Framework.Web
{
    public class BrowserCacheActionResult : ActionResult
    {
        /// <summary>
        /// 资源过期时间
        /// </summary>
        const int _durationInDays = 30;

        /// <summary>
        /// Etag
        /// </summary>
        public string _etagCacheKey = string.Empty;

        /// <summary>
        /// Last-Mofiy
        /// </summary>
        public DateTime _lastModify = NetworkTime.Now;

        public Func<object> _valueFactory = null;


        public BrowserCacheActionResult(DateTime lastModifyFactory, string versionFactory, Func<object> valueFactory)
        {
            _lastModify = lastModifyFactory;
            _etagCacheKey = versionFactory;
            _valueFactory = valueFactory;
        }

        public BrowserCacheActionResult(Func<DateTime> lastModifyFactory, Func<string> versionFactory, Func<object> valueFactory)
        {
            _lastModify = lastModifyFactory();
            _etagCacheKey = versionFactory();
            _valueFactory = valueFactory;
        }

        protected bool IsInBrowserCache(HttpContextBase httpContext)
        {
            string inputEtag = httpContext.Request.Headers["If-None-Match"];
            DateTime lastModify = DateTime.MinValue;
            DateTime.TryParse(httpContext.Request.Headers["If-Modified-Since"], out lastModify);

            //Etag
            if (string.Equals(inputEtag, _etagCacheKey, StringComparison.Ordinal))
            {
                httpContext.Response.Cache.SetETag(inputEtag);
                httpContext.Response.Cache.SetLastModified(_lastModify);
                //httpContext.Response.AppendHeader("Content-Length", "0");
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                return true;
            }
            //Last-Modified
            if (lastModify != DateTime.MinValue && DateTime.Equals(lastModify, _lastModify))
            {
                httpContext.Response.Cache.SetLastModified(lastModify);
                httpContext.Response.Cache.SetETag(_etagCacheKey);
                //httpContext.Response.AppendHeader("Content-Length", "0");
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                return true;
            }
            return false;
        }

        protected void SendOuptToClient(HttpContextBase context)
        {
            HttpResponseBase response = context.Response;
            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetETag(_etagCacheKey);
            cache.SetLastModified(_lastModify);
            cache.SetExpires(NetworkTime.Now.AddDays(_durationInDays)); // For HTTP 1.0 browsers
            cache.SetOmitVaryStar(true);
            cache.SetMaxAge(TimeSpan.FromDays(_durationInDays));
            //cache.SetLastModified(NetworkTime.Now);

            cache.SetValidUntilExpires(true);
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.VaryByHeaders["Accept-Encoding"] = true;// Tell proxy to cache different versions depending on Accept-Encoding
            response.ContentType = "application/json";
            if (response.IsClientConnected)
                response.Flush();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;

            if (IsInBrowserCache(context.HttpContext))
            {
                return;
            }

            SendOuptToClient(context.HttpContext);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var value = _valueFactory();
            if (value != null)
            {
                var callbackMethodName = context.HttpContext.Request.Params["jsoncallback"];
                var output = string.Empty;
                if (!string.IsNullOrEmpty(callbackMethodName))
                    output = string.Format(CultureInfo.CurrentCulture, "{0}({1});", callbackMethodName, serializer.Serialize(value));
                else
                    output = JsonConverter.ToJson(value);
                response.Write(output);
            }

            //response.End();
        }
    }
}
