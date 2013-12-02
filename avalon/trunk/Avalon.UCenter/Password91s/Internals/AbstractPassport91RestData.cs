using Avalon.HttpClient;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UCenter
{
    internal abstract class AbstractPassport91RestData
    {
        public string Action { get; set; }

        public string TimeStamp { get; set; }

        /// <summary>
        /// 调用程序的APPNAME
        /// </summary>
        public string UserName { get; set; }

        public string CheckCode { get; set; }

        public string Format { get; set; }

        protected Passport91RestDataResult<T> InvokeApi<T>(string url)
        {
            ApiHttpClient client = new ApiHttpClient();
            return client.HttpPost<Passport91RestDataResult<T>>(url, this);
        }
    }

    internal class Passport91RestDataResult<T>
    {
        public T Code { get; set; }

        public string Message { get; set; }

        public string ServerTime { get; set; }

        public string ClientIp { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
