using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Projects.Tool.OAuth
{
    /// <summary>
    /// OAuth异常
    /// </summary>
    public class OAuthException : Exception
    {
        public OAuthException(string Message)
            : base(Message)
        {
        }

        public OAuthException(string Message, Exception InnerException)
            : base(Message, InnerException)
        {
        }
    }

    /// <summary>
    /// OAuth授权异常
    /// </summary>
    public class OAuthUnauthorizedException : OAuthException
    {
        public string ResponseText { get; private set; }

        public OAuthUnauthorizedException(string message, string responseText)
            : base(message)
        {
            ResponseText = responseText;
        }

        public OAuthUnauthorizedException(string message, string responseText, Exception innerException)
            : base(message, innerException)
        {
            ResponseText = responseText;
        }
    }

    /// <summary>
    /// OAuth网络异常
    /// </summary>
    public class OAuthNetworkException : OAuthException
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; private set; }

        /// <summary>
        /// HTTP状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string ResponseText { get; private set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestArgs { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="url"></param>
        /// <param name="statusCode"></param>
        /// <param name="responseText"></param>
        /// <param name="requestArgs"></param>
        public OAuthNetworkException(string message, string url, HttpStatusCode statusCode, string responseText, string requestArgs)
            : base(message)
        {
            RequestUrl = url;
            StatusCode = statusCode;
            ResponseText = responseText;
            RequestArgs = requestArgs;
        }
    }
}
