using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

using Projects.Tool.Util;

namespace Projects.Tool.Http
{
    public class HttpClient
    {
        public HttpClient()
        {
            this.Timeout = 30000;
        }

        public HttpClient(int timeout, WebHeaderCollection headers = null, 
            Encoding encoding = null, string host = null)
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
            using (var client = CreateWebClient())
            {
                return client.DownloadString(url);
            }
        }

        public string HttpGet(string url, string key, string value)
        {
            url = new UriPathBuilder(url).Append(key, value).ToString();
            return HttpGet(url);
        }

        public string HttpGet(string url, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpGet(url);
        }

        public string HttpGet(string url, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
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
            using (var client = CreateWebClient())
            {
                var buffer = client.UploadValues(url, data);
                return client.Encoding.GetString(buffer);
            }
        }

        public string HttpPost(string url, object values)
        {
            var data = UriPathBuilder.ToNameValueCollection(values);
            return HttpPost(url, data);
        }

        public virtual string HttpPostJson(string url, object instance)
        {
            Arguments.NotNull(instance, "instance");

            var json = JsonConverter.ToJson(instance);
            using (var client = CreateWebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                return client.UploadString(url, json);
            }
        }

        public string HttpPostJson(string url, object instance, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return HttpPostJson(url, instance);
        }

        public string HttpPostJson(string url, object instance, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpPostJson(url, instance);
        }

        public virtual string HttpUploadFile(string url, string filename)
        {
            using (var client = CreateWebClient())
            {
                var buffer = client.UploadFile(url, filename);
                return client.Encoding.GetString(buffer);
            }
        }

        public string HttpUploadFile(string url, string filename, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return HttpUploadFile(url, filename);
        }

        public string HttpUploadFile(string url, string filename, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpUploadFile(url, filename);
        }
    }
}
