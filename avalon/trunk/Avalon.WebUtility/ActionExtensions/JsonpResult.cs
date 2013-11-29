using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Avalon.WebUtility
{
    /// <summary>
    /// jsonp返回message，则封装
    /// </summary>
    public class JsonpMessage {
        public string Message { get; set; }

        public JsonpMessage(string message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// JSONP扩展
    /// </summary>
    public class JsonpResult : ActionResult
    {
        private static JavaScriptSerializer _serializer = new JavaScriptSerializer();

        public JsonpResult()
        {
        }

        public JsonpResult(object data)
        {
            Data = data;
        }

        public JsonpResult(object data, Encoding encoding)
        {
            Data = data;
            ContentEncoding = encoding;
        }

        public Encoding ContentEncoding { get; set; }

        public object Data { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/x-javascript";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                string callbackMethodName = context.HttpContext.Request.Params["jsoncallback"];
                string output = string.Empty;
                if (!string.IsNullOrEmpty(callbackMethodName))
                {
                    output = string.Format(CultureInfo.CurrentCulture, "{0}({1});", callbackMethodName, Serialize(Data, ContentEncoding));
                }
                else
                {
                    output = Serialize(Data, ContentEncoding);
                }
                response.Write(output);
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private string Serialize(object data, Encoding encoding)
        {
            return _serializer.Serialize(data);
        }
    }
}
