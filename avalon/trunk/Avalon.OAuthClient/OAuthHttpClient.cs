
using Avalon.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Avalon.OAuthClient
{
    /// <summary>
    /// 封装 OAuth 请求的客户端调用库
    /// </summary>
    public class OAuthHttpClient : OpenApiHttpClient
    {
        public OAuthHttpClient()
            : base()
        {
        }

        public OAuthHttpClient(int timeout, WebHeaderCollection headers = null, Encoding encoding = null, string host = null)
            : base(timeout, headers, encoding, host)
        {
        }

        public OAuthContext OAuthContext
        {
            get { return OAuthContext.Current; }
        }

        protected override Uri OnCreateWebRequestUri(Uri uri)
        {
            uri = OAuthService.OAuthProvider.OnOpenApiRequest(uri);

            return base.OnCreateWebRequestUri(uri);
        }
    }
}
