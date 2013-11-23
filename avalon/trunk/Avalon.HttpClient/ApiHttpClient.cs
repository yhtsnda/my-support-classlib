using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Avalon.Utility;

namespace Avalon.HttpClient
{
    public class ApiHttpClient : HttpClient
    {
        public ApiHttpClient()
            : base()
        {
        }

        public ApiHttpClient(int timeout, WebHeaderCollection headers = null, Encoding encoding = null, string host = null)
            : base(timeout, headers, encoding, host)
        {
        }

        public T HttpGet<T>(string url)
        {
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, string key, object value)
        {
            url = UriPath.AppendArguments(url, key, value);
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
            return HttpGetInner<T>(url);
        }

        public T HttpGet<T>(string url, object values)
        {
            url = UriPath.AppendArguments(url, values);
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
            return HttpPostInner<T>(url, UriPath.ToNameValueCollection(values));
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
            url = UriPath.AppendArguments(url, key, value);
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpPostJson<T>(string url, object instance, object values)
        {
            url = UriPath.AppendArguments(url, values);
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpPostJson<T>(string url, object instance, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
            return HttpPostJsonInner<T>(url, instance);
        }

        public T HttpUploadFile<T>(string url, string filename)
        {
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, byte[] fileBytes)
        {
            return HttpUploadFileInner<T>(url, fileBytes);
        }

        public T HttpUploadFile<T>(string url, string filename, string key, object value)
        {
            url = UriPath.AppendArguments(url, key, value);
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, string filename, object values)
        {
            url = UriPath.AppendArguments(url, values);
            return HttpUploadFileInner<T>(url, filename);
        }

        public T HttpUploadFile<T>(string url, string filename, NameValueCollection data)
        {
            url = UriPath.AppendArguments(url, data);
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

        protected virtual T HttpUploadFileInner<T>(string url, byte[] fileBytes)
        {
            return ParseResult<T>(HttpUploadFile(url, fileBytes));
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
