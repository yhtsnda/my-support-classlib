using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Projects.Tool.Http
{
    public class TimeoutWebClient : WebClient
    {
        private int TimeOut;

        public TimeoutWebClient(int timeout)
        {
            this.TimeOut = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = this.TimeOut;
            return request;
        }
    }
}
