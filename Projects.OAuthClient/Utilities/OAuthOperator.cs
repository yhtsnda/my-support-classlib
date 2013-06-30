using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Projects.Tool;
using Projects.Tool.Http;

namespace Projects.OAuthClient
{
    /// <summary>
    /// OAuth授权的内部实现类
    /// </summary>
    internal static class OAuthOperator
    {
        private static OpenApiHttpClient client = new OpenApiHttpClient();

        /// <summary>
        /// 获取服务端的认证码
        /// </summary>
        /// <param name="authorizeData">认证数据</param>
        /// <returns>认证码</returns>
        public static OpenApiResult<AuthorizationCode> GetCode(AuthorizeData authorizeData)
        {
            var url = UriPathBuilder.Combine(OAuthContext.OAuthServicePath, "token");

            var data = new NameValueCollection();
            authorizeData.UpdateDatas(data);

            return client.HttpPostForResult<AuthorizationCode>(url, data);
        }

        /// <summary>
        /// 获取服务端的Token
        /// </summary>
        /// <param name="tokenData">Token数据</param>
        /// <returns>授权对象</returns>
        public static OpenApiResult<AccessGrant> GetToken(SimpleTokenData tokenData)
        {
            var url = UriPathBuilder.Combine(OAuthContext.OAuthServicePath, "token");

            var data = new NameValueCollection();
            tokenData.UpdateDatas(data);

            return client.HttpPostForResult<AccessGrant>(url, data);
        }

        /// <summary>
        /// 获取服务端授权对象
        /// </summary>
        /// <param name="accessToken">存取凭据</param>
        /// <returns>授权对象</returns>
        public static AccessGrant GetAccessGent(string accessToken)
        {
            Arguments.NotNullOrEmpty(accessToken, "accessToken");

            var url = UriPathBuilder.Combine(OAuthContext.OAuthServicePath, "valid");
            return client.HttpGet<AccessGrant>(url, Protocal.ACCESS_TOKEN, accessToken);
        }
    }
}
