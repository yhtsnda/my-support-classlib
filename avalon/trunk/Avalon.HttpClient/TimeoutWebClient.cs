using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Avalon.HttpClient
{
    public class TimeoutWebClient : WebClient
    {
        int timeout;
        public TimeoutWebClient(int timeout)
        {
            this.timeout = timeout;
            Encoding = Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = timeout;
            return request;
        }
    }
}
