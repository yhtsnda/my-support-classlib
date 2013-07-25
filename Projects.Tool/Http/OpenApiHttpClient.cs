using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;

using Projects.Tool.Util;

namespace Projects.Tool.Http
{
    public class OpenApiHttpClient : ApiHttpClient
    {
        public OpenApiHttpClient()
            : base()
        {
        }

        public OpenApiHttpClient(int timeout, WebHeaderCollection headers = null, Encoding encoding = null, string host = null)
            : base(timeout, headers, encoding, host)
        {
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url)
        {
            return base.HttpGetInner<OpenApiResult<T>>(url);
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, string key, string value)
        {
            url = new UriPathBuilder(url).Append(key, value).ToString();
            return base.HttpGetInner<OpenApiResult<T>>(url);
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return base.HttpGetInner<OpenApiResult<T>>(url);
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return base.HttpGetInner<OpenApiResult<T>>(url);
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, string key, string value)
        {
            var data = new NameValueCollection();
            data.Add(key, value);
            return base.HttpPostInner<OpenApiResult<T>>(url, data);
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, object values)
        {
            var data = UriPathBuilder.ToNameValueCollection(values);
            return base.HttpPostInner<OpenApiResult<T>>(url, data);
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, NameValueCollection data)
        {
            return base.HttpPostInner<OpenApiResult<T>>(url, data);
        }

        public virtual OpenApiResult<T> HttpPostJsonForResult<T>(string url, object instance)
        {
            return base.HttpPostJsonInner<OpenApiResult<T>>(url, instance);
        }

        public virtual OpenApiResult<T> HttpPostJsonForResult<T>(string url, object instance, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return base.HttpPostJsonInner<OpenApiResult<T>>(url, instance);
        }

        public virtual OpenApiResult<T> HttpPostJsonForResult<T>(string url, object instance, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return base.HttpPostJsonInner<OpenApiResult<T>>(url, instance);
        }

        public virtual OpenApiResult<T> UploadFileForResult<T>(string url, string filename)
        {
            return base.HttpUploadFileInner<OpenApiResult<T>>(url, filename);
        }

        public virtual OpenApiResult<T> UploadFileForResult<T>(string url, string filename, object values)
        {
            url = new UriPathBuilder(url).AppendMany(values).ToString();
            return base.HttpUploadFileInner<OpenApiResult<T>>(url, filename);
        }

        public virtual OpenApiResult<T> UploadFileForResult<T>(string url, string filename, NameValueCollection data)
        {
            url = new UriPathBuilder(url).AppendMany(data).ToString();
            return base.HttpUploadFileInner<OpenApiResult<T>>(url, filename);
        }

        protected override T HttpGetInner<T>(string url)
        {
            var result = base.HttpGetInner<OpenApiResult<T>>(url);
            return ProcessResult<T>(result, url);
        }

        protected override T HttpPostInner<T>(string url, NameValueCollection data)
        {
            var result = base.HttpPostInner<OpenApiResult<T>>(url, data);
            return ProcessResult<T>(result, url);
        }

        protected override T HttpPostJsonInner<T>(string url, object instance)
        {
            var result = base.HttpPostJsonInner<OpenApiResult<T>>(url, instance);
            return ProcessResult<T>(result, url);
        }

        protected override T HttpUploadFileInner<T>(string url, string filename)
        {
            var result = base.HttpUploadFileInner<OpenApiResult<T>>(url, filename);
            return ProcessResult<T>(result, url);
        }

        protected virtual T ProcessResult<T>(OpenApiResult<T> result, string url)
        {
            if (result.Code == 0)
                return result.Data;

            throw new Exception("服务器调用 " + url + " 发生异常: \r\n" + result.Message);
        }
    }

    public class OpenApiResult<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
