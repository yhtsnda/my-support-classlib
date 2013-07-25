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
            UpdateClient();
            var url = UriPathBuilder.Combine(OAuthService.OAuthServicePath, "token");

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
            UpdateClient();
            var url = UriPathBuilder.Combine(OAuthService.OAuthServicePath, "token");

            var data = new NameValueCollection();
            tokenData.UpdateDatas(data);
            var innerResult = client.HttpPostForResult<AccessGrant.InnerAccessGrant>(url, data);

            var result = new OpenApiResult<AccessGrant>() { Code = innerResult.Code, Message = innerResult.Message };
            if (innerResult.Data != null)
                result.Data = innerResult.Data.Convert();
            return result;
        }

        /// <summary>
        /// 获取服务端授权对象
        /// </summary>
        /// <param name="accessToken">存取凭据</param>
        /// <returns>授权对象</returns>
        public static AccessGrant GetAccessGent(string accessToken)
        {
            UpdateClient();
            Arguments.NotNullOrEmpty(accessToken, "accessToken");

            var url = UriPathBuilder.Combine(OAuthService.OAuthServicePath, "valid");
            var innerResult = client.HttpGetForResult<AccessGrant.InnerAccessGrant>(url, Protocal.ACCESS_TOKEN, accessToken);
            if (innerResult.Code == 0)
                return innerResult.Data.Convert();

            return null;
        }

        private static void UpdateClient()
        {
            client.Host = OAuthService.OAuthServiceHost;
        }
    }
}
