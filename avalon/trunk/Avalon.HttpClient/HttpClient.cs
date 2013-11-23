using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using Avalon.Profiler;
using Avalon.Utility;

namespace Avalon.HttpClient
{
    public class HttpClient
    {
        public HttpClient()
        {
            Timeout = 30000;
        }

        public HttpClient(int timeout, WebHeaderCollection headers = null, Encoding encoding = null, string host = null)
            : this()
        {
            Timeout = timeout;
            Headers = headers;
            Encoding = encoding;
        }

        public virtual int Timeout { get; set; }

        public virtual Encoding Encoding { get; set; }

        public virtual WebHeaderCollection Headers { get; set; }

        public virtual string Host { get; set; }

        protected virtual WebClient CreateWebClient()
        {
            var client = new InnerWebClient(Timeout, OnCreateWebRequestUri, OnGetWebRequest, OnGetWebResponse);
            if (Headers != null)
            {
                for (var i = 0; i < Headers.Count; i++)
                {
                    client.Headers.Add(Headers.GetKey(i), Headers.Get(i));
                }
            }
            if (Encoding != null)
            {
                client.Encoding = Encoding;
            }
            return client;
        }

        protected virtual Uri OnCreateWebRequestUri(Uri uri)
        {
            return uri;
        }

        protected virtual void OnGetWebRequest(WebRequest request)
        {
            if (request is HttpWebRequest && !String.IsNullOrEmpty(Host))
            {
                ((HttpWebRequest)request).Host = Host;
            }
        }

        protected virtual void OnGetWebResponse(WebResponse response)
        {
        }

        public virtual string HttpGet(string url)
        {
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("http", "get:" + url);

            using (var client = CreateWebClient())
            {
                try
                {
                    using (ProfilerContext.Watch("get:" + url))
                        return client.DownloadString(url);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("请求地址 {0} 发生错误。", url), ex);
                }
            }
        }

        public string HttpGet(string url, string key, string value)
        {
            url = UriPath.AppendArguments(url, key, value);
            return HttpGet(url);
        }

        public string HttpGet(string url, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
            return HttpGet(url);
        }

        public string HttpGet(string url, object values)
        {
            url = UriPath.AppendArguments(url, values);
            return HttpGet(url);
        }

        public string HttpPost(string url, string key, string value)
        {
            var data = new NameValueCollection();
            data.Add(key, value);
            return HttpPost(url, data);
        }

        public virtual string HttpPost(string url, NameValueCollection data)
        {
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("http", "post:" + url);

            using (var client = CreateWebClient())
            {
                try
                {
                    var values = UriPath.ConstructQueryString(data);
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                    using (ProfilerContext.Watch("post:" + url))
                        return client.UploadString(url, values);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("请求地址 {0} 发生错误。", url), ex);
                }
            }
        }

        public string HttpPost(string url, object values)
        {
            var data = UriPath.ToNameValueCollection(values);
            return HttpPost(url, data);
        }

        public virtual string HttpPostJson(string url, object instance)
        {
            Arguments.NotNull(instance, "instance");

            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("http", "postjson:" + url);

            var json = JsonConverter.ToJson(instance);
            using (var client = CreateWebClient())
            {
                try
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    using (ProfilerContext.Watch("postjson:" + url))
                        return client.UploadString(url, json);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("请求地址 {0} 发生错误。", url), ex);
                }
            }
        }

        public string HttpPostJson(string url, object instance, object values)
        {
            url = UriPath.AppendArguments(url, values);
            return HttpPostJson(url, instance);
        }

        public string HttpPostJson(string url, object instance, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
            return HttpPostJson(url, instance);
        }

        public virtual string HttpUploadFile(string url, string filename)
        {
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("http", "uploadfile:" + url);

            using (var client = CreateWebClient())
            {
                try
                {
                    var buffer = client.UploadFile(url, filename);
                    using (ProfilerContext.Watch("uploadfile:" + url))
                        return client.Encoding.GetString(buffer);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("请求地址 {0} 发生错误。", url), ex);
                }
            }
        }

        public virtual string HttpUploadFile(string url, byte[] fileBytes)
        {
            if (ProfilerContext.Current.Enabled)
                ProfilerContext.Current.Trace("http", "uploadbytes:" + url);

            using (var client = CreateWebClient())
            {
                try
                {
                    var buffer = client.UploadData(url, fileBytes);
                    using (ProfilerContext.Watch("uploadbytes:" + url))
                        return client.Encoding.GetString(buffer);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("请求地址 {0} 发生错误。", url), ex);
                }
            }
        }

        public string HttpUploadFile(string url, string filename, object values)
        {
            url = UriPath.AppendArguments(url, values);
            return HttpUploadFile(url, filename);
        }

        public string HttpUploadFile(string url, string filename, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
            return HttpUploadFile(url, filename);
        }
    }
}
