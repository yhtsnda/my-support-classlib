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
        private Action<WebResponse> onGetWebResponseHandler;

        public InnerWebClient(int timeout,
            Func<Uri, Uri> onCreateWebRequestUriHandler,
            Action<WebRequest> onGetWebRequestHandler,
            Action<WebResponse> onGetWebResponseHandler)
        {
            this.Timeout = timeout;
            this.onCreateWebRequestUriHandler = onCreateWebRequestUriHandler;
            this.onGetWebRequestHandler = onGetWebRequestHandler;
            this.onGetWebResponseHandler = onGetWebResponseHandler;
            Encoding = Encoding.UTF8;
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
            onGetWebResponseHandler(response);
            return response;
        }
    }
}
