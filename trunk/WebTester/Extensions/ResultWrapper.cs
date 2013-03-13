using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

using Projects.Framework;

namespace WebTester.Extensions
{
    public class ResultWrapper<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ResultWrapper(int code, string message)
        {
            if (code == 0)
                throw new PlatformException("接口返回值错误的情况下Code不能为0");
            if (string.IsNullOrEmpty(message))
                throw new PlatformException("接口返回值错误的情况下Message不能为空");

            Code = code;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public ResultWrapper(T data)
        {
            if (data == null)
                throw new PlatformException("接口正常的情况下Data不能为空");

            Code = 0;
            Message = string.Empty;
            Data = data;
        }

        public bool IsSuccess()
        {
            return this.Code == 0;
        }
    }

    public class TokenHelper
    {
        /// <summary>
        /// 输出服务端错误
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public static void ResponseError(HttpContextBase httpContext, string message, int code)
        {
            var data = new ResultWrapper<object>(code, message);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            httpContext.Response.ContentType = "application/json";
            var callback = httpContext.Request["callback"];
            var content = serializer.Serialize(data);
            if (!string.IsNullOrEmpty(callback))
                content = callback + "(" + serializer.Serialize(data) + ")";

            httpContext.Response.Write(content);
        }
    }
}