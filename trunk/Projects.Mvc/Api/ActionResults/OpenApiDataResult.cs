using Projects.Tool.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Projects.Framework.Web
{
    public class OpenApiDataResult : ActionResult
    {
        public OpenApiData Data { get; set; }

        public  OpenApiDataResult(object data)
        {
            Data = new OpenApiData {Data = data};
        }

        public OpenApiDataResult(Exception exception, int code, object data = null)
        {
            Data = new OpenApiData {Code = code, Message = exception.Message, Data = data};
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            string callback;
            if (JsonpResultImpl.IsJsonpRequest(context.HttpContext.Request, out callback))
                JsonpResultImpl.Write(context.HttpContext.Response, callback, Data);
            else
            {
                response.ContentType = "application/json;charset=utf-8";
                response.Write(JsonConverter.ToJson(Data));
            }
        }

        public class OpenApiData
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }
}
