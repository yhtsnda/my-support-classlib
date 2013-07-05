using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Projects.Tool;

namespace Projects.Framework.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OpenApiAttribute : ApiAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            object ar = filterContext.HttpContext.Items[ApiControllerActionInvoker.ActionResultInvoker];
            if (ar == null)
            {
                filterContext.Result = new OpenApiDataResult(filterContext.Exception, GetResponseCode(filterContext));
                filterContext.ExceptionHandled = true;
            }
            base.OnException(filterContext);
        }
    }
}
