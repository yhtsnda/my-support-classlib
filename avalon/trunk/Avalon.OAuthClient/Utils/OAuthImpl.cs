using Avalon.HttpClient;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Avalon.OAuthClient
{
    public static class OAuthImpl
    {
        static OpenApiHttpClient openapiClient = new OpenApiHttpClient();

        static void UpdateClient()
        {
            openapiClient.Host = OAuthService.OAuthServiceHost;
        }

        public static OpenApiResult<AuthorizationCode> GetCode(AuthorizeData authorizeData)
        {
            UpdateClient();

            var url = UriPath.Combine(OAuthService.OAuthServicePath, "token");

            var data = new NameValueCollection();
            authorizeData.UpdateDatas(data);

            return openapiClient.HttpPostForResult<AuthorizationCode>(url, data);
        }

        public static OpenApiResult<AccessGrant> GetToken(TokenData tokenData)
        {
            UpdateClient();

            var url = UriPath.Combine(OAuthService.OAuthServicePath, "token");

            var data = new NameValueCollection();
            tokenData.UpdateDatas(data);

            var innerResult = openapiClient.HttpPostForResult<AccessGrant.InnerAccessGrant>(url, data);

            var result = new OpenApiResult<AccessGrant>() { Code = innerResult.Code, Message = innerResult.Message };
            if (innerResult.Data != null)
                result.Data = innerResult.Data.Convert();
            return result;
        }

        public static AccessGrant GetAccessGrant(string accessToken)
        {
            UpdateClient();

            Arguments.NotNullOrEmpty(accessToken, "accessToken");

            var url = UriPath.Combine(OAuthService.OAuthServicePath, "valid");
            var innerResult = openapiClient.HttpGetForResult<AccessGrant.InnerAccessGrant>(url, Protocal.access_token, accessToken);
            if (innerResult.Code == 0 && innerResult.Data != null)
                return innerResult.Data.Convert();

            return null;
        }
    }
}
