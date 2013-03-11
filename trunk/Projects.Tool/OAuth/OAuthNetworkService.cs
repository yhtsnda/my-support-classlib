using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Tool.Util;
using System.Net;
using System.IO;

namespace Projects.Tool.OAuth
{
    /// <summary>
    /// OAuth网络服务
    /// </summary>
    public class OAuthNetworkService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string MakeHttpRequest(HttpMethod method, string url, string args = null,
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
                request.ContentType = string.Format("application/x-www-form-urlencoded");
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

                throw new OAuthNetworkException("OAuth请求过程中发生网络异常", request.RequestUri.ToString(), response.StatusCode, responseData, args);
            }
            catch (Exception ex)
            {
                throw new OAuthException("OAuth验证过程发生错误", ex);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return responseData;
        }
    }
}
