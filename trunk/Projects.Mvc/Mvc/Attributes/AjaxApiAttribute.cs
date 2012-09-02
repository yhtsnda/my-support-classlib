using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BuildingSiteCheck.Mvc;

namespace BuildingSiteCheck.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxApiAttribute : ApiAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new AjaxApiDataResult(filterContext.Exception, GetResponseCode(filterContext));
            filterContext.ExceptionHandled = true;
        }
    }
}
