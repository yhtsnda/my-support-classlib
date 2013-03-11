using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Projects.Tool.Util;

namespace Projects.Framework
{
    public class WebRequestHelper
    {
        /// <summary>
        /// 以Post方式异步提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static void PostAsync(string url, NameValueCollection postData)
        {
            string error;
            PostAsync(url, postData, out error);
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return;
        }

        /// <summary>
        /// 以Post方式异步提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static void PostAsync(string url, NameValueCollection postData,DownloadStringCompletedEventHandler handler)
        {
            string error;
            PostAsync(url, postData,out error,handler);
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return;
        }

        /// <summary>
        /// 以Post方式异步提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static void PostAsync(string url, NameValueCollection postData, out string error)
        {
            ProcessWebException(() =>
            {
                var wc = new WebClient();
                wc.UploadValuesAsync(new Uri(url), postData);
                return true;
            }
            , out error);
            return;
        }

        /// <summary>
        /// 以Post方式异步提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static void PostAsync(string url, NameValueCollection postData, out string error, DownloadStringCompletedEventHandler handler)
        {
            ProcessWebException(() =>
            {
                var wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(handler);
                wc.UploadValuesAsync(new Uri(url), postData);
                return true;
            }
            , out error);
            return;
        }

        /// <summary>
        /// 以Post方式请求Json数据，返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static T Post<T>(string url, NameValueCollection postData, out string error)
        {
            return ProcessWebException(() =>
                                           {
                                               var wc = new WebClient();
                                               byte[] data =
                                                   wc.UploadValues(url, "post", postData);
                                               string strData = Encoding.UTF8.GetString(data);
                                               CheckResult(strData);//添加检查是否在职教育那边已经报错
                                               var entity = JsonConverter.FromJson<T>(strData); //JsonConvert.DeserializeObject<T>(strData);
                                               return entity;
                                           }
                                       , out error);
        }

        /// <summary>
        /// 以Post方式请求Json数据，返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static T Post<T>(string url, NameValueCollection postData)
        {
            string error;
            var t = Post<T>(url, postData, out error);
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return t;
        }

        /// <summary>
        /// 以Post方式提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static void Post(string url, NameValueCollection postData)
        {
            string error;
            Post(url, postData, out error);
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return;
        }

        /// <summary>
        /// 以Post方式提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static void Post(string url, NameValueCollection postData, out string error)
        {
            ProcessWebException(() =>
                                    {
                                        var wc = new WebClient();
                                        wc.UploadValues(url, "post", postData);
                                        return true;
                                    }
                                , out error);
        }

        /// <summary>
        /// 以Get方式请求Json数据，返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(string url)
        {
            string error;
            var t = Get<T>(url, out error);

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return t;
        }

        /// <summary>
        /// 以Get方式请求Json数据，返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static T Get<T>(string url, out string error)
        {
            return ProcessWebException(() =>
                                           {
                                               var wc = new WebClient();
                                               byte[] data =
                                                   wc.DownloadData(url);
                                               string strData = Encoding.UTF8.GetString(data);
                                               if (string.IsNullOrEmpty(strData))
                                                   return default(T);

                                               var entity = JsonConverter.FromJson<T>(strData);
                                               return entity;
                                           }
                                       , out error);
        }

        /// <summary>
        /// 以Get方式提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void Get(string url)
        {
            string error;
            Get(url, out error);

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return;
        }

        /// <summary>
        /// 以Get方式提交数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static void Get(string url, out string error)
        {
            ProcessWebException(() =>
            {
                var wc = new WebClient();
                wc.DownloadData(url);
                return true;
            }
                                , out error);
        }


        public static T GetRemoteResponse<T>(string url, string method, string body, string contentType = "application/x-www-form-urlencoded")
        {
            var responseData = GetRemoteResponse(url, method, body, contentType);
            if (responseData.StatusCode == HttpStatusCode.OK)
            {
                return JsonConverter.FromJson<T>(responseData.ReceivedData);
            }
            else
            {
                throw new ArgumentException(responseData.Error);
            }
        }

        public static ResponseData GetRemoteResponse(string url, string method, string body, string contentType)
        {
            var responseData = new ResponseData { };
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = method;
                httpRequest.ContentType = contentType;
                httpRequest.Timeout = 360000; //响应超时值,默认30秒
                httpRequest.KeepAlive = true;
                httpRequest.ContentLength = 0;
                httpRequest.Accept = "*/*";
                //请求的内容不为空
                if (!string.IsNullOrEmpty(body))
                {
                    //设置请求资源
                    byte[] arrReqData = System.Text.Encoding.UTF8.GetBytes(body);
                    httpRequest.ContentLength = arrReqData.Length;
                    Stream reqStream = httpRequest.GetRequestStream();
                    reqStream.Write(arrReqData, 0, arrReqData.Length);
                }
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                {
                    if (httpResponse != null)
                        responseData = GetNormalResponse(httpResponse);
                }
            }
            catch (WebException wex)//响应异常处理
            {
                responseData = GetWebExceptionResponse(wex);
            }
            catch (Exception ex) //其它异常处理
            {
                responseData.IsSucceed = false;
                responseData.StatusCode = 0;
                responseData.Error = ex.Message;
            }

            return responseData;
        }

        /// <summary>
        /// 从HttpWebResponse中获取响应内容
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private static string GetResponseContent(HttpWebResponse httpResponse)
        {
            string content = null;

            using (Stream stream = httpResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    StreamReader sr = new StreamReader(stream);
                    content = sr.ReadToEnd();
                }
            }

            return content;
        }

        private static ResponseData GetNormalResponse(HttpWebResponse httpResponse)
        {
            var responseData = new ResponseData { };
            responseData.StatusCode = httpResponse.StatusCode;
            responseData.IsSucceed = true;
            responseData.ReceivedData = GetResponseContent(httpResponse);

            return responseData;
        }

        private static ResponseData GetWebExceptionResponse(WebException wex)
        {
            var responseData = new ResponseData { };
            if (wex.Status == WebExceptionStatus.ProtocolError) //服务器有响应,反馈错误码及信息.
            {
                if (wex.Response != null)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)wex.Response;
                    responseData.StatusCode = httpResponse.StatusCode;
                    responseData.ReceivedData = GetResponseContent(httpResponse);
                    responseData.IsSucceed = true;

                    try
                    {
                        var result = JsonConverter.FromJson(responseData.ReceivedData, typeof(ResponseError)) as ResponseError;
                        if (result != null)
                        {
                            //如果响应的是与规范相合的错误信息，则记录错误内容
                            responseData.Error = result.Error ?? result.Msg;
                        }
                        else
                        {
                            //如果响应的是其他内容，则直接记录最初的错误内容

                            responseData.Error = wex.Message;
                        }
                    }
                    catch (Exception)
                    {
                        responseData.Error = wex.Message;
                    }

                }
            }
            else //网络异常,如超时
            {
                responseData.StatusCode = (HttpStatusCode)wex.Status;
                responseData.Error = wex.Message;
                if (wex.Response != null && wex.Response.ContentLength > 0)
                {
                    StreamReader sr = new StreamReader(wex.Response.GetResponseStream());
                    responseData.ReceivedData = sr.ReadToEnd();
                }
                responseData.IsSucceed = false;
            }

            return responseData;
        }

        /// <summary>
        /// 请求错误捕获
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="func">执行的函数</param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static T ProcessWebException<T>(Func<T> func, out string error)
        {
            string message;
            Exception exception;

            try
            {
                error = null;
                return func();
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    try
                    {
                        //获取网页相应的错误文本
                        string rt = new StreamReader(we.Response.GetResponseStream()).ReadToEnd();
                        var result = JsonConverter.FromJson(rt, typeof(ResponseError)) as ResponseError;
                        //JsonConvert.DeserializeObject(rt, typeof(ResponseError)) as ResponseError;
                        if (result != null)
                        {
                            //如果响应的是与规范相合的错误信息，则记录错误内容
                            error = result.Error ?? result.Msg;
                            return default(T);
                        }
                        else
                        {
                            //如果响应的是其他内容，则直接记录最初的错误内容
                            message = we.Message;
                            exception = we;
                        }
                    }
                    catch (Exception)
                    {
                        message = we.Message;
                        exception = we;
                    }
                }
                else
                {
                    message = we.Message;
                    exception = we;
                }
            }
            //错误处理
            throw new Exception(message, exception);
        }

        private static void CheckResult(string result)
        {
            if (result.Contains("{\"error\":\""))
            {
                string message = result.Substring(10, result.Length - 12);
                throw new Exception(message);
            }
        }
    }


    /// <summary>
    /// 请求错误实体
    /// </summary>
    internal class ResponseError
    {
        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 错误具体信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 错误数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Uap错误消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 请求响应数据
    /// </summary>
    public class ResponseData
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 请求收到的数据
        /// </summary>
        public string ReceivedData { get; set; }
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSucceed { get; set; }
    }
}
