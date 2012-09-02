using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace Projects.Tool
{
    /// <summary>
    /// HttpBase类提供了对http请求的封装,Encoding.UTF8
    /// </summary>
    public class HttpBase
    {
        //原始代码来自 http://www.codeproject.com/KB/IP/httpwebrequest_response.aspx

        private string _userName;
        private string _password;
        private int _timeout;
        private NameValueCollection _requestHeaders = new NameValueCollection();
        private NameValueCollection _responseHeaders = new NameValueCollection();
        private Encoding _encoding = Encoding.UTF8;
        private WebProxy _proxy;
        private HttpWebResponse _webResponse;
        private HttpStatusCode _responseStatusCode;
        private bool _allowAutoRedirect;
        private NameValueCollection _responseCookies;
        //private NameValueCollection _keyValuedData = new NameValueCollection();

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection ResponseCookies
        {
            get { return _responseCookies; }
            //set { _responseCookies = value; }
        }

        /// <summary>
        /// 2010-12-30 增加，应对302请求的 location 表头
        /// </summary>
        public bool AllowAutoRedirect
        {
            get { return _allowAutoRedirect; }
            set { _allowAutoRedirect = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// <summary>
        /// 代理 2010-12-25 增加 by 杜有发
        /// </summary>
        public WebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        /// <summary>
        /// 2010-12-26增加，取代out status的方式
        /// </summary>
        public HttpStatusCode ResponseStatusCode
        {
            get { return _responseStatusCode; }
            //set { _responseStatusCode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public HttpWebResponse WebResponse
        {
            get { return _webResponse; }
            set { _webResponse = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection RequestHeaders
        {
            get { return _requestHeaders; }
            set { _requestHeaders = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public NameValueCollection ResponseHeaders
        {
            get { return _responseHeaders; }
            set { _responseHeaders = value; }
        }

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        public HttpBase()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public HttpBase(int timeout)
        {

        }

        #endregion

        /// <summary>
        /// 创建Web请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method">Http请求方式:"POST","GET"</param>
        /// <param name="requestBody"></param>
        /// <returns>请求实例</returns>
        public virtual HttpWebRequest CreateWebRequest(string url, string method, string requestBody)
        {
            //bool isRequestBodyEmpty = string.IsNullOrEmpty(requestBody);

            #region fix 当get时，url要附加requestBody

            if (!string.IsNullOrEmpty(requestBody))//2010-07-01增加，如果是get，则不允许input stream
            {
                if (method.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (url.IndexOf("?") > -1)//把参数附加上去
                        url += "&" + requestBody;
                    else
                        url += "?" + requestBody;

                    requestBody = null;//清空input stream
                }
            }

            #endregion

            HttpWebRequest webRequest = null;
            if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ProtocolVersion = HttpVersion.Version10;
            }
            //普通的http请求 
            else
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url); ;
            }

            //增加处理Https的情况
            webRequest.KeepAlive = false;
            webRequest.Method = method;
            webRequest.ServicePoint.Expect100Continue = false;//去掉 100-continue 的表头 2010-12-23
            

            //timeout
            if (_timeout > 0)//2010-09-26 增加 by 杜有发
                webRequest.Timeout = _timeout;

            //_allowAutoRedirect
            webRequest.AllowAutoRedirect = _allowAutoRedirect;

            //代理设置
            if (_proxy != null)
                webRequest.Proxy = _proxy;

            //附加header
            if (_requestHeaders.Count > 0)
            {
                //先处理特殊属性,Referer,UserAgent等属性，特殊，否则，此标头必须使用适当的属性进行修改 Referer
                //string[] attributions = new string[] { "Referer","UserAgent",""};
                string tempKey = "Referer";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.Referer = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "User-Agent";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.UserAgent = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "Accept";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.Accept = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "Connection";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.Connection = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "Content-Length";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    _requestHeaders.Remove(tempKey);//直接移出
                }
                tempKey = "Content-Type";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.ContentType = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "Expect";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.Expect = _requestHeaders[tempKey];//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
                tempKey = "If-Modified-Since";
                if (!string.IsNullOrEmpty(_requestHeaders[tempKey]))
                {
                    webRequest.IfModifiedSince = DateTime.Parse(_requestHeaders[tempKey]).ToUniversalTime();//2010-12-22 增加 by 杜有发
                    _requestHeaders.Remove(tempKey);
                }
            }
            //附加普通报文头
            int iCount = _requestHeaders.Count;
            string key, value;
            for (int i = 0; i < iCount; i++)
            {
                key = _requestHeaders.Keys[i];
                value = _requestHeaders[i];

                webRequest.Headers.Add(key, value);
            }
            
            ////proxy
            //if (!string.IsNullOrEmpty(_proxyServer))
            //    webRequest.Proxy = new WebProxy(_proxyServer, _proxyPort);

            //credentialCache
            if (!string.IsNullOrEmpty(_userName))
            {
                CredentialCache wrCache = new CredentialCache();
                wrCache.Add(new Uri(url), "Basic", new NetworkCredential(_userName, _password));
                webRequest.Credentials = wrCache;
            }
            else//2010-06-24增加,避免了 PUT 请求方法的 401 Unauthorized 错误
                webRequest.Credentials = CredentialCache.DefaultCredentials;

            #region 取消下列CookieContainer的做法，该做法无法使用RequestHeaders.Add("Cookie",cookie);2011-01-04 增加 by 杜有发
            ////CookieContainer 2011-01-04 增加 
            //CookieContainer cookieContainer = new CookieContainer();
            //webRequest.CookieContainer = cookieContainer;

            //if (!string.IsNullOrEmpty(_requestHeaders["Cookie"]))
            //{
            //    cookieContainer.SetCookies(webRequest.RequestUri, _requestHeaders["Cookie"]);
            //}
            #endregion

            //准备上传的数据
            if (!string.IsNullOrEmpty(requestBody))
            {
                //避免get时 产生“无法发送具有此谓词类型的内容正文”错误  add by sky pan 2010-7-27
                if (webRequest.Method.StartsWith("p", StringComparison.CurrentCultureIgnoreCase))//2010-10-21简单判断，以p开头便执行
                {
                    if (string.IsNullOrEmpty(webRequest.ContentType))//为了兼容请求web service的post请求，Content-Type: text/xml; charset=utf-8
                        webRequest.ContentType = "application/x-www-form-urlencoded";

                    byte[] bytes = _encoding.GetBytes(requestBody); //Console.WriteLine(_requestData);
                    webRequest.ContentLength = bytes.Length;

                    Stream oStreamOut = webRequest.GetRequestStream();
                    oStreamOut.Write(bytes, 0, bytes.Length);
                    oStreamOut.Close();

                    //Console.WriteLine("yes");
                }
            }
            else //2010-06-21增加
                webRequest.ContentLength = 0;//LengthRequired

            return webRequest;
        }

        /// <summary>
        /// 获取最终响应，2010-09-30 增加HttpWebResponse by 杜有发
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public virtual string GetResponseText(string url, string method, string requestBody)
        {
            return _encoding.GetString(GetResponse(url, method, requestBody));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public virtual byte[] GetResponse(string url, string method, string requestBody)
        {
            _webResponse = null;
            string exString = null;

            HttpWebRequest webRequest = CreateWebRequest(url, method, requestBody);

            try
            {
                _webResponse = (HttpWebResponse)webRequest.GetResponse();

                //cookie
                string allCookieString = _webResponse.Headers["Set-Cookie"];
                if (!string.IsNullOrEmpty(allCookieString))
                {
                    _responseCookies = new NameValueCollection();

                    string[] cookies = allCookieString.Split(',');//cookie里的过期GMT时间也是含有“,”号的
                    foreach (string cookieString in cookies)
                    { 
                        //c = “tgc=TGT-MTExNDMwODg3Nw==-1294129295-87B74BCB1356E809B9CECFCF0B60E297; domain=login.sina.com.cn; path=/; Httponly”
                        string[] cookie = cookieString.Split(';');
                        if (cookie.Length > 0)
                        {
                            string kv = cookie[0];//第一个是：tgc=TGT-MTExNDMwODg3Nw==-1294129295-87B74BCB1356E809B9CECFCF0B60E297
                            
                            int p = kv.IndexOf('=');//第一个=号的位置，value里也是可能含有等号的
                            if (p > -1)//kv有可能是cookie的过期时间的分隔：“expires=Tuesday, 11-Jan-11 08:21:36 GMT;”，所以这里要判断
                            {
                                string key = kv.Substring(0, p);
                                string value = kv.Substring(p + 1);

                                _responseCookies.Add(key, value);
                            }
                            //else
                                //Console.WriteLine(kv);
                        }
                    }

                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    _webResponse = ex.Response as HttpWebResponse;
                }
                else
                    exString = ex.ToString();
            }

            if (_webResponse != null)
            {
                //状态
                _responseStatusCode = _webResponse.StatusCode;

                //响应报文
                _responseHeaders = _webResponse.Headers; //_responseHeaders = new NameValueCollection(webResponse.Headers);

                //响应内容
                Stream stream = _webResponse.GetResponseStream();//你这是网络流,肯定不支持查找操作 

                byte[] rtn = StreamToBytes(stream, _webResponse.ContentLength);

                //关闭
                stream.Close();
                _webResponse.Close();

                return rtn;
            }
            else
            {
                _responseStatusCode = HttpStatusCode.InternalServerError;

                return _encoding.GetBytes(exString);
            }
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] StreamToBytes(Stream stream, long initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public string Post(string url, string requestBody)
        {
            return GetResponseText(url, "POST", requestBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Get(string url)
        {
            return Get(url, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public string Get(string url, string requestBody)
        {
            return GetResponseText(url, "GET", requestBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public string Put(string url, string requestBody)
        {
            return GetResponseText(url, "PUT", requestBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public string Delete(string url, string requestBody)
        {
            return GetResponseText(url, "DELETE", requestBody);
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
