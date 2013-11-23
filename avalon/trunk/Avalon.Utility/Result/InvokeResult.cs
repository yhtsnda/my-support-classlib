using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class InvokeResult<TData>
    {
        public InvokeResult()
        {
            Code = ResultCode.OK;
        }

        public InvokeResult(TData data)
        {
            Data = data;
            Code = ResultCode.OK;
        }

        public InvokeResult(int code, string error, Exception exception = null)
        {
            Code = code;
            Message = error;
            Error = error;
            Exception = exception;
        }

        public TData Data { get; set; }

        public int Code { get; set; }

        [Obsolete("请使用 Error 属性")]
        public string Message
        {
            get { return Error; }
            set { Error = value; }
        }

        public string Error { get; set; }

        //Exception 序列化会引起问题，add by skypan 2011年6月21日14:29:15
        [System.Web.Script.Serialization.ScriptIgnore]
        public Exception Exception { get; set; }

        public bool IsSuccess()
        {
            return Code == ResultCode.OK;
        }

        public InvokeResult<TOther> Cast<TOther>()
        {
            return new InvokeResult<TOther>(Code, Message, Exception);
        }

        public InvokeResult<TOther> Cast<TOther>(Func<TData, TOther> func)
        {
            return new InvokeResult<TOther>(func(Data));
        }
    }

    public class ResultCode
    {
        /// <summary>
        /// 指示请求成功，且请求的信息包含在响应中。 这是最常接收的状态代码。
        /// </summary>
        public const int OK = 200000;
        /// <summary>
        /// 指示服务器未能识别请求。如果没有其他适用的错误，或者如果不知道准确的错误或错误没有自己的错误代码，则发送 BadRequest。
        /// </summary>
        public const int BadRequest = 400000;
        /// <summary>
        /// 指示请求的资源要求身份验证。
        /// </summary>
        public const int Unauthorized = 401000;
        /// <summary>
        /// 指示服务器拒绝满足请求。
        /// </summary>
        public const int Forbidden = 403000;
        /// <summary>
        /// 指示请求的资源不在服务器上。
        /// </summary>
        public const int NotFound = 404000;
        /// <summary>
        /// 指示请求的资源上不允许请求方法（POST 或 GET）。
        /// </summary>
        public const int MethodNotAllowed = 405000;
        /// <summary>
        ///  指示服务器上发生了一般错误。
        /// </summary>
        public const int InternalServerError = 500000;
    }
}

