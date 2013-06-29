using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Projects.Tool.Http
{
    public class InnerWebClient : WebClient
    {
        private int Timeout;
        private Func<Uri, Uri> onCreateWebRequestUriHandler;
        private Action<WebRequest> onGetWebRequestHandler;
        private Action<WebRequest> onGetWebResponseHandler;

        public InnerWebClient(int timeout,
            Func<Uri, Uri> onCreateWebRequestUriHandler,
            Action<WebRequest> onGetWebRequestHandler,
            Action<WebRequest> onGetWebResponseHandler)
        {
            this.Timeout = timeout;
            this.onCreateWebRequestUriHandler = onCreateWebRequestUriHandler;
            this.onGetWebRequestHandler = onGetWebRequestHandler;
            this.onGetWebResponseHandler = onGetWebResponseHandler;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            address = onCreateWebRequestUriHandler(address);
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = this.Timeout;
            onGetWebRequestHandler(request);
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            onGetWebResponseHandler(request);
            return response;
        }
    }
}
