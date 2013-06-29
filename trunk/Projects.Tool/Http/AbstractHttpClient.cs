using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

using Projects.Tool.Util;


namespace Projects.Tool.Http
{
    public  abstract class AbstractHttpClient
    {
        public abstract int Timeout { get; set; }

        protected WebClient CreateWebClient()
        {
            return new InnerWebClient(Timeout, OnCreateWebRequestUri, OnGetWebRequest, OnGetWebResponse);
        }

        protected abstract Uri OnCreateWebRequestUri(Uri uri);
        protected abstract void OnGetWebRequest(WebRequest request);
        protected abstract void OnGetWebResponse(WebRequest request);

        public abstract T HttpGet<T>(string url) where T : class;
        public abstract T HttpGet<T>(string url, string key, string value) where T : class;
        public abstract T HttpGet<T>(string url, NameValueCollection data) where T : class;
        public abstract T HttpGet<T>(string url, object values) where T : class;
        public abstract T HttpPost<T>(string url, NameValueCollection data) where T : class;
        public abstract T HttpPost<T>(string url, string key, string value) where T : class;
        public abstract T HttpPost<T>(string url, object values) where T : class;
        public abstract T HttpPostJson<T>(string url, object instance, object values) where T : class;
        public abstract T HttpPostJson<T>(string url, object instance, NameValueCollection data = null) where T : class;
        public abstract T UploadFile<T>(string url, string fileName, object values) where T : class;
        public abstract T UploadFile<T>(string url, string fileName, NameValueCollection data = null) where T : class;
    }
}
