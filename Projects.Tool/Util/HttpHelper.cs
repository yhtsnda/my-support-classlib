using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 
    /// </summary>
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    /// <summary>
    /// HTTP异常
    /// </summary>
    public class HttpException : Exception
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
        public HttpException(string message, string url, HttpStatusCode statusCode, string responseText, string requestArgs)
            : base(message)
        {
            RequestUrl = url;
            StatusCode = statusCode;
            ResponseText = responseText;
            RequestArgs = requestArgs;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly string _unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// 执行HTTP请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetResponse(HttpMethod method, string url, string args = null,
            IEnumerable<KeyValuePair<string, string>> headers = null, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var requestUrl = string.Empty;
            if (!string.IsNullOrEmpty(args) && method == HttpMethod.GET)
            {
                if (url.IndexOf("?") >= 0)
                    requestUrl = url + "&" + args;
                else
                    requestUrl = url + "?" + args;
            }
            else
            {
                requestUrl = url;
            }

            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 1000;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 20000;
            request.Method = method.ToString();
            //设置ContentType
            if (method == HttpMethod.POST)
                request.ContentType = string.Format("application/x-www-form-urlencoded;charset={0}", encoding.WebName);
            else
                request.ContentType = string.Format("text/html;charset={0}", encoding.WebName);
            //设置http headers
            if (headers != null && headers.Count() > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            string responseData = string.Empty;
            HttpWebResponse response = null;
            try
            {
                //设置post请求的数据
                if (!string.IsNullOrEmpty(args) && method != HttpMethod.GET)
                {
                    var argBytes = encoding.GetBytes(args);
                    request.ContentLength = argBytes.Length;

                    var stream = request.GetRequestStream();
                    stream.Write(argBytes, 0, argBytes.Length);
                    if (stream != null)
                        stream.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd();
                }
            }
            catch (WebException we)
            {
                response = we.Response as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd();
                }

                throw new HttpException(we.Message, request.RequestUri.ToString(), response.StatusCode, responseData, args);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return responseData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T GetResponse<T>(HttpMethod method, string url, string args = null,
            IEnumerable<KeyValuePair<string, string>> headers = null, Encoding encoding = null)
        {
            try
            {
                var result = GetResponse(method, url, args, headers, encoding);
                return JsonConverter.FromJson<T>(result);
            }
            catch (HttpException httpEx)
            {
                throw httpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 不同于HttpUtility.UrlEncode，这个方法返回编码的特殊字符是大写的。
        /// HttpUtility.UrlEncode("=")返回值是：%3d，而这个方法则返回%3D。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncode(string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var encodeBuilder = new StringBuilder();
            var resultBytes = encoding.GetBytes(value);
            foreach (var symbol in resultBytes)
            {
                if (_unreservedChars.IndexOf((char)symbol) != -1)
                    encodeBuilder.Append((char)symbol);
                else
                    encodeBuilder.Append('%' + Convert.ToString((char)symbol, 16).ToUpper());
            }
            return encodeBuilder.ToString();
        }

        /// <summary>
        /// 参数求MD5哈希
        /// </summary>
        public static string EncodeParams(NameValueCollection nameValues)
        {
            List<Tuple<string, string>> lists = new List<Tuple<string, string>>();
            foreach (string key in nameValues.Keys)
                lists.Add(new Tuple<string, string>(key, UrlEncode((string)nameValues[key])));

            lists = lists.OrderBy(o => o.Item1).ThenBy(o => o.Item2).ToList();

            string arguments = String.Join("&", lists.Select(o => o.Item1 + "=" + o.Item2));
            MD5 md5 = new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(arguments));
            return BitConverter.ToString(bytes);
        }
    }
}
