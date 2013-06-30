using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Projects.Tool.Http;

namespace Projects.OAuthClient
{
    /// <summary>
    /// 封装OAuth请求的客户端类
    /// </summary>
    public class OAuthHttpClient : OpenApiHttpClient
    {
        public OAuthContext OAuthContext
        {
            get { return OAuthContext.Current; }
        }
    }
}
