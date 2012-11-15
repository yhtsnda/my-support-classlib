using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Projects.Mvc
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

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if (Exception != null)
                if (Code >= 1000)
                    response.StatusCode = Code / 1000;
                else
                    response.StatusCode = Code;
            response.ContentType = "application/json";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (Exception != null)
            {
                response.Write(serializer.Serialize(new
                {
                    code = Code,
                    error = Exception.Message,
                    exception = Exception.ToString()
                }));
            }
            else
            {
                response.Write(serializer.Serialize(this.Data));
            }
        }
    }
}
