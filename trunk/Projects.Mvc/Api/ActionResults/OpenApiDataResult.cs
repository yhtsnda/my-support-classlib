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

        public OpenApiDataResult(Exception exception, int code)
        {
            Data = new OpenApiData {Code = code, Message = exception.Message};
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
                throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            response.Write(serializer.Serialize(Data));
        }

        public class OpenApiData
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
    }
}
