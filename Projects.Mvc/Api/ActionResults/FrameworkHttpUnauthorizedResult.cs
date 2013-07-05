using System;
using System.Web.Mvc;

namespace Projects.Framework.Web
{
    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class FrameworkHttpUnauthorizedResult : HttpUnauthorizedResult
    {
        public FrameworkHttpUnauthorizedResult()
            : base()
        { }

        public FrameworkHttpUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        { }
    }

    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class FrameworkApiUnauthorizedResult : JsonResult
    {
        public string Error { get; set; }

        public int Code { get; set; }

        public Exception Exception { get; set; }

        public FrameworkApiUnauthorizedResult()
            : this("未授权")
        { }

        public FrameworkApiUnauthorizedResult(string error, Exception exception = null)
        {
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            this.Error = error;
            this.Exception = exception;
            this.Code = 401;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.Data = new Projects.Tool.InvokeResult<object>(this.Code, this.Error, this.Exception);
            base.ExecuteResult(context);
        }
    }

    public class PlatformHttpUnauthorizedException : Exception
    {
        public PlatformHttpUnauthorizedException()
            : base()
        { }

        public PlatformHttpUnauthorizedException(string message)
            : base(message)
        { }

        public PlatformHttpUnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
