using Projects.Tool.Util;
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Projects.Framework.Web
{
    public class AjaxApiDataResult : ActionResult
    {
        public AjaxApiDataResult(object data)
        {
            this.Data = data;
        }

        public AjaxApiDataResult(Exception exception, int code)
        {
            this.Exception = exception;
            this.Code = code;
        }

        public object Data
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public int Code
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = "application/json; charset=utf-8";

            var data = Exception == null ? Data : new
            {
                code = Code,
                error = Exception.Message,
                exception = Exception.ToString()
            };

            if (Exception != null)
            {
                var code = Code >= 1000 ? Code / 1000 : Code;
                if (code < 400 || code > 599)
                    code = 500;
                response.StatusCode = code;
            }

            response.Write(JsonConverter.ToJson(data));
        }
    }
}
