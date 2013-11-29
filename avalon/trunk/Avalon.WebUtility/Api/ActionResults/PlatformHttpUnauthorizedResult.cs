using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class PlatformHttpUnauthorizedResult : HttpUnauthorizedResult
    {
        public PlatformHttpUnauthorizedResult()
            : base()
        { }

        public PlatformHttpUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        { }
    }

    /// <summary>
    /// 未授权的返回结果
    /// </summary>
    public class PlatformApiUnauthorizedResult : JsonResult
    {
        public string Error { get; set; }

        public int Code { get; set; }

        public Exception Exception { get; set; }

        public PlatformApiUnauthorizedResult()
            : this("未授权")
        { }

        public PlatformApiUnauthorizedResult(string error, Exception exception = null)
        {
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet;
            this.Error = error;
            this.Exception = exception;
            this.Code = 401;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.Data = new Avalon.Utility.InvokeResult<object>(this.Code, this.Error, this.Exception);
            base.ExecuteResult(context);
        }
    }
}