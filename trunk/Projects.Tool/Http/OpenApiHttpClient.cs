using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;

using Projects.Tool.Util;

namespace Projects.Tool.Http
{
    public class OpenApiHttpClient<T> : AbstractHttpClient
    {
        public override int Timeout { get; set; }

        public OpenApiHttpClient()
        {
            this.Timeout = 3000;
        }

        protected override Uri OnCreateWebRequestUri(Uri uri)
        {
            return uri;
        }

        protected override void OnGetWebRequest(WebRequest request)
        {
        }

        protected override void OnGetWebResponse(WebRequest request)
        {
        }

        public override T HttpGet<T>(string url)
        {
            return ProcessResult(HttpGetForResult<T>(url), url);
        }

        public override T HttpGet<T>(string url, string key, string value)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.Append(key, value);
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpGet<T>(string url, NameValueCollection data)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.AppendMany(data);
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpGet<T>(string url, object values)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.AppendMany(UriBuilder.ToNameValueCollection(values));
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpPost<T>(string url, NameValueCollection data)
        {
            return ProcessResult(HttpPostForResult<T>(url, data), url);
        }

        public override T HttpPost<T>(string url, string key, string value)
        {
            return ProcessResult(HttpPostForResult<T>(url, key, value), url);
        }

        public override T HttpPost<T>(string url, object values)
        {
             return ProcessResult(HttpPostForResult<T>(url, values), url);
        }

        public override T HttpPostJson<T>(string url, object instance, object values)
        {
            return ProcessResult(HttpPostJsonForResult<T>(url, instance, values), url);
        }

        public override T HttpPostJson<T>(string url, object instance, NameValueCollection data = null)
        {
            return ProcessResult(HttpPostJsonForResult<T>(url, instance, data), url);
        }

        public override T UploadFile<T>(string url, string fileName, object values)
        {
            return ProcessResult(UploadFileForResult<T>(url, fileName, values), url);
        }

        public override T UploadFile<T>(string url, string fileName, NameValueCollection data = null)
        {
            return ProcessResult(UploadFileForResult<T>(url, fileName, data), url);
        }

         public virtual OpenApiResult<T> HttpGetForResult<T>(string url)
        {
            Func<OpenApiResult<T>> func = () =>
            {
                using (var client = CreateWebClient())
                {
                    return ParseOpenApiResult<T>(client.DownloadString(url));
                }
            };
            return InvokeResult<T>(func);
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, string key, string value)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.Append(key, value);
            return HttpGetForResult<T>(builder.ToString());
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, NameValueCollection data)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.AppendMany(data);
            return HttpGetForResult<T>(builder.ToString());
        }

        public virtual OpenApiResult<T> HttpGetForResult<T>(string url, object values)
        {
            return HttpGetForResult<T>(url, UriBuilder.ToNameValueCollection(values));
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, string key, string value)
        {
            var data = new NameValueCollection();
            data.Add(key, value);
            return HttpPostForResult<T>(url, data);
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, object values)
        {
            return HttpPostForResult<T>(url, UriBuilder.ToNameValueCollection(values));
        }

        public virtual OpenApiResult<T> HttpPostForResult<T>(string url, NameValueCollection data)
        {
            Func<OpenApiResult<T>> func = () =>
            {
                using (var client = CreateWebClient())
                {
                    var buffer = client.UploadValues(url, "POST", data);
                    return ParseOpenApiResult<T>(Encoding.UTF8.GetString(buffer));
                }
            };
            return InvokeResult<T>(func);
        }

        public virtual OpenApiResult<T> HttpPostJsonForResult<T>(string url, object instance, object values)
        {
            return HttpPostJsonForResult<T>(url, instance, UriBuilder.ToNameValueCollection(values));
        }

        public virtual OpenApiResult<T> HttpPostJsonForResult<T>(string url, object instance, NameValueCollection data = null)
        {
            Func<OpenApiResult<T>> func = () =>
            {
                Arguments.NotNull(instance, "instance");

                var json = JsonConverter.ToJson(instance);
                using (var client = CreateWebClient())
                {
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    return ParseOpenApiResult<T>(client.UploadString(url, json));
                }
            };
            return InvokeResult<T>(func);
        }

        public virtual OpenApiResult<T> UploadFileForResult<T>(string url, string filename, object values)
        {
            return UploadFileForResult<T>(url, filename, UriBuilder.ToNameValueCollection(values));
        }

        public virtual OpenApiResult<T> UploadFileForResult<T>(string url, string filename, NameValueCollection data = null)
        {
            Func<OpenApiResult<T>> func = () =>
            {
                UriBuilder builder = new UriBuilder(url);
                builder.AppendMany(data);
                url = builder.ToString();
                using (var client = CreateWebClient())
                {
                    var buffer = client.UploadFile(url, filename);
                    return ParseOpenApiResult<T>(Encoding.UTF8.GetString(buffer));
                }
            };
            return InvokeResult<T>(func);
        }

        protected virtual OpenApiResult<T> ParseOpenApiResult<T>(string json)
        {
            if (String.IsNullOrEmpty(json) || json == "null")
                return new OpenApiResult<T>() { Data = default(T) };

            return JsonConverter.FromJson<OpenApiResult<T>>(json);
        }

        protected virtual T ProcessResult<T>(OpenApiResult<T> result, string url)
        {
            if (result.Code == 0)
                return result.Data;
            throw new Exception("服务器调用 " + url + " 发生异常: \r\n" + result.Message);
        }

        protected virtual OpenApiResult<T> InvokeResult<T>(Func<OpenApiResult<T>> func)
        {
            return func();
        }
    }

    public class OpenApiResult<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
