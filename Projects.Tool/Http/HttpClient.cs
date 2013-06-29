using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

using Projects.Tool.Util;

namespace Projects.Tool.Http
{
    public class HttpClient : AbstractHttpClient
    {
        public override int Timeout { get; set; }
        public HttpClient()
        {
            this.Timeout = 300;
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

        /// <summary>
        /// 以GET的方式获取数据
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>结果</returns>
        public override T HttpGet<T>(string url)
        {
            using (var client = CreateWebClient())
            {
                try
                {
                    return client.DownloadString(url) as T;
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("无法转换指定的类型");
                }
            }
        }

        public override T HttpGet<T>(string url, string key, string value)
        {
            UriBuilder builder = new UriBuilder(url);
            builder.Append(key, value);
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpGet<T>(string url, NameValueCollection data)
        {
            UriBuilder builder = new UriBuilder(url).AppendMany(data);
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpGet<T>(string url, object values)
        {
            UriBuilder builder = new UriBuilder(url).AppendMany(UriBuilder.ToNameValueCollection(values));
            return HttpGet<T>(builder.ToString());
        }

        public override T HttpPost<T>(string url, NameValueCollection data)
        {
            using (var client = CreateWebClient())
            {
                var buffer = client.UploadValues(url, data);
                try
                {
                    return Encoding.UTF8.GetString(buffer) as T;
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("无法转换指定的类型");
                }
            }
        }

        public override T HttpPost<T>(string url, string key, string value)
        {
            var data = new NameValueCollection();
            data.Add(key, value);
            return HttpPost<T>(url, data);
        }

        public override T HttpPost<T>(string url, object values)
        {
            return HttpPost<T>(url, UriBuilder.ToNameValueCollection(values));
        }

        public override T HttpPostJson<T>(string url, object instance, object values)
        {
            return HttpPostJson<T>(url, instance, UriBuilder.ToNameValueCollection(values));
        }

        public override T HttpPostJson<T>(string url, object instance, NameValueCollection data = null)
        {
            Arguments.NotNull(instance, "instance");

            UriBuilder builder = new UriBuilder(url);
            if (data != null)
                builder.AppendMany(data);

            var json = JsonConverter.ToJson(instance);
            using (var client = CreateWebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                try
                {
                    return client.UploadString(builder.ToString(), json) as T;
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("无法转换指定的类型");
                }
            }
        }

        public override T UploadFile<T>(string url, string fileName, object values)
        {
            return UploadFile<T>(url, fileName, UriBuilder.ToNameValueCollection(values));
        }

        public override T UploadFile<T>(string url, string fileName, NameValueCollection data = null)
        {
            UriBuilder builder = new UriBuilder(url);
            if (data != null)
                builder.AppendMany(data);

            using (var client = CreateWebClient())
            {
                var buffer = client.UploadFile(builder.ToString(), fileName);
                try
                {
                    return Encoding.UTF8.GetString(buffer) as T;
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("无法转换指定的类型");
                }
            }
        }
    }
}
