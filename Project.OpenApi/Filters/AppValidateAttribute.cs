using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projects.UserCenter;

namespace Projects.OpenApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class AppValidateAttribute : ActionFilterAttribute
    {
        private static readonly string AppParameter = "app";
        private static readonly string TerminalParameter = "terminal";
        private static readonly string SourceParameter = "source";

        public AppValidateAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int appId = 0, terminalId = 0, source = 0;
            Int32.TryParse(filterContext.RouteData.Values[AppParameter].ToString(), out appId);
            Int32.TryParse(filterContext.RouteData.Values[TerminalParameter].ToString(), out terminalId);
            Int32.TryParse(filterContext.RouteData.Values[SourceParameter].ToString(), out source);

            var clientService = Projects.Framework.DependencyResolver.Resolve<ClientAppService>();
            var clientApp = clientService.GetClientApp(appId);
            if (clientApp == null)
                throw new ArgumentException("应用不存在, ClientAppId=" + appId);
            if (clientApp.Status == ClientAppStatus.Disabled)
                throw new ArgumentException("应用已被禁用 ClientAppId=" + appId);

            if (filterContext.ActionParameters.ContainsKey("appId"))
                filterContext.ActionParameters["appId"] = appId;
            if (filterContext.ActionParameters.ContainsKey("terminalId"))
                filterContext.ActionParameters["terminalId"] = terminalId;
            if (filterContext.ActionParameters.ContainsKey("source"))
                filterContext.ActionParameters["source"] = source;
            if (filterContext.ActionParameters.ContainsKey("platcode"))
                filterContext.ActionParameters["platcode"] = appId * 100000000 + terminalId * 10000 + source;

            base.OnActionExecuting(filterContext);
        }
    }
}