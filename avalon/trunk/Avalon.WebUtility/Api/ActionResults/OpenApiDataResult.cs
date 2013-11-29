using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;
using Avalon.Utility;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 接口结果
    /// </summary>
    public class OpenApiDataResult : ActionResult
    {
        public OpenApiDataResult(object data)
        {
            Data = new OpenApiData() { Data = data };
        }

        public OpenApiDataResult(Exception exception, int code, object data = null)
        {
            Data = new OpenApiData() { Code = code, Message = GetExceptionMessage(exception), Data = data };
        }

        /// <summary>
        /// 数据
        /// </summary>
        public OpenApiData Data
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            string callback;
            if (JsonpResultImpl.IsJsonpRequest(context.HttpContext.Request, out callback))
            {
                JsonpResultImpl.Write(context.HttpContext.Response, callback, Data);
            }
            else
            {
                response.ContentType = "application/json;charset=utf-8";
                response.Write(JsonConverter.ToJson(Data));
            }
        }

        /// <summary>
        /// 结果数据
        /// </summary>
        public class OpenApiData
        {
            /// <summary>
            /// 码
            /// </summary>
            public int Code { get; set; }

            /// <summary>
            /// 消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public object Data { get; set; }
        }

        string GetExceptionMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.Message);
            ex = ex.InnerException;
            while (ex != null)
            {
                sb.Append("\t" + ex.Message);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }
    }
}
