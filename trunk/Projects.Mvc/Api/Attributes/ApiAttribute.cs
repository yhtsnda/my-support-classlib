using System;
using System.Web.Mvc;
using Projects.Tool;

namespace Projects.Framework.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiAttribute : FilterAttribute
    {
        ILog log = LogManager.GetLogger<ApiAttribute>();

        protected int GetResponseCode(ExceptionContext filterContext)
        {
            int code = ResultCode.InternalServerError;
            if (filterContext.Exception is ArgumentException)
                code = ResultCode.BadRequest;

            if (filterContext.Exception is PlatformException)
                code = ((PlatformException)filterContext.Exception).Code;
            return code;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (log.IsErrorEnabled)
                log.Error(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}
