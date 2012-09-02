using System;
using System.Web.Mvc;
using Projects.Tool;

namespace BuildingSiteCheck.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiAttribute : FilterAttribute, IExceptionFilter
    {
        protected int GetResponseCode(ExceptionContext filterContext)
        {
            var code = ResultCode.InternalServerError;
            if (filterContext.Exception is ArgumentException)
                code = ResultCode.BadRequest;

            var ex = filterContext.Exception as BuildingSiteCheckException;
            if (ex != null)
            {
                code = ex.Code;
                LogManager.GetLogger("building_log").Error("系统出现:" + ex.Code.ToString() + "错误~", ex);
            }

            return code;
        }

        public abstract void OnException(ExceptionContext filterContext);
    }
}
