using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxApiAttribute : ApiAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            object ar = filterContext.HttpContext.Items[ApiControllerActionInvoker.ActionResultInvoker];
            if (ar == null)
            {
                filterContext.Result = new AjaxApiDataResult(filterContext.Exception, GetResponseCode(filterContext));
                filterContext.ExceptionHandled = true;
            }
            base.OnException(filterContext);
        }
    }
}
