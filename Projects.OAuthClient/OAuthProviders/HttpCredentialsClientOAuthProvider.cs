using Projects.Tool;
using Projects.Tool.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Projects.OAuthClient
{
    /// <summary>
    /// HTTP 下使用信任客户端模式的 OAuth 服务提供
    /// </summary>
    /// <remarks>
    /// 信任客户端模式使用相同的授权进行数据的请求，但会附带当前的 userid。
    /// </remarks>
    public class HttpCredentialsClientOAuthProvider : HttpAbstractOAuthProvider
    {
        public override Uri OnOpenApiRequest(Uri uri)
        {
            var accessToken = GetCurrent().AppAccessToken;
            uri = GetUri(uri, accessToken);

            var context = HttpContext.Current;

            if (String.IsNullOrEmpty(HttpUtility.ParseQueryString(uri.Query)["userId"]))
            {
                if (context.IsAvailable() && context.User.Identity.IsAuthenticated)
                    uri = GetUri(uri, "userid", context.User.Identity.Name);
            }
            return uri;
        }
    }
}
