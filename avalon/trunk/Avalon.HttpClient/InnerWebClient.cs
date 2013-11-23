using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Avalon.HttpClient
{
    internal class InnerWebClient : WebClient
    {
        int timeout;
        Func<Uri, Uri> onCreateWebRequestUriHandler;
        Action<WebRequest> onGetWebRequestHandler;
        Action<WebResponse> onGetWebResponseHandler;

        public InnerWebClient(int timeout, Func<Uri, Uri> onCreateWebRequestUriHandler, Action<WebRequest> onGetWebRequestHandler, Action<WebResponse> onGetWebResponseHandler)
        {
            this.timeout = timeout;
            this.onCreateWebRequestUriHandler = onCreateWebRequestUriHandler;
            this.onGetWebRequestHandler = onGetWebRequestHandler;
            this.onGetWebResponseHandler = onGetWebResponseHandler;

            Encoding = Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            address = onCreateWebRequestUriHandler(address);
            WebRequest request = base.GetWebRequest(address);
            request.Timeout = timeout;
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
