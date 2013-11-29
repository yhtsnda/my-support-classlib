using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Avalon.Utility;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiAttribute : FilterAttribute, IExceptionFilter
    {
        ILog log = LogManager.GetLogger<ApiAttribute>();

        protected int GetResponseCode(ExceptionContext filterContext)
        {
            int code = ResultCode.InternalServerError;
            if (filterContext.Exception is ArgumentException)
                code = ResultCode.BadRequest;

            if (filterContext.Exception is AvalonException)
                code = ((AvalonException)filterContext.Exception).Code;
            return code;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (log.IsErrorEnabled)
                log.Error(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}