using Projects.Tool.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Projects.Tool.Http
{
    public class ApiHttpClient : HttpClient
    {
        public ApiHttpClient()
            : base()
        {
        }

        public ApiHttpClient(int timeout, WebHeaderCollection headers = null, Encoding encoding = null,
            string host = null)
            : base(timeout, headers, encoding, host)
        {
        }

        public T HttpGet<T>(string url)
        {
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, string key, object value)
        {
            url = new UriPathBuilder(url).Append(key, value).ToString();
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return HttpGetInner<T>(url);
        }

        public T HttpPost<T>(string url, string key, object value)
        {
            NameValueCollection data = new NameValueCollection();
            data.Add(key, value.ToString());
            return HttpPostInner<T>(url, data);
        }

        public T HttpPost<T>(string url, object values)
        {
            return HttpPostInner<T>(url, UriPathBuilder.ToNameValueCollection(values));
        }

        public virtual T HttpPost<T>(string url, NameValueCollection data)
        {
            return HttpPostInner<T>(url, data);
        }

        public T HttpPostJson<T>(string url, object instance)
        {
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpPostJson<T>(string url, object instance, string key, object value)
        {
            url = new UriPathBuilder(url).Append(key, value).ToString();
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpPostJson<T>(string url, object instance, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpPostJson<T>(string url, object instance, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpUploadFile<T>(string url, string filename)
        {
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, string filename, string key, object value)
        {
            url = new UriPathBuilder(url).Append(key, value).ToString();
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, string filename, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, string filename, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return HttpUploadFileInner<T>(url, filename);
        }

        protected virtual T HttpGetInner<T>(string url)
        {
            return ParseResult<T>(HttpGet(url));
        }

        protected virtual T HttpPostInner<T>(string url, NameValueCollection data)
        {
            return ParseResult<T>(HttpPost(url, data));
        }

        protected virtual T HttpPostJsonInner<T>(string url, object instance)
        {
            return ParseResult<T>(HttpPostJson(url, instance));
        }

        protected virtual T HttpUploadFileInner<T>(string url, string filename)
        {
            return ParseResult<T>(HttpUploadFile(url, filename));
        }

        protected virtual T ParseResult<T>(string json)
        {
            if (String.IsNullOrEmpty(json) || json == "null")
                return default(T);

            try
            {
                return JsonConverter.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Join("解析类型 {0} 的文本 {1} 发生异常。", typeof(T).FullName, json), ex);
            }
        }
    }
}
