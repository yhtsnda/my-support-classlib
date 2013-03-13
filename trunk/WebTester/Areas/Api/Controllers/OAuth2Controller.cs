using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Framework.OAuth2;
using WebTester.Controllers;

namespace WebTester.Areas.Api.Controllers
{
    public class OAuth2Controller : OpenApiBaseController
    {
        [ActionName("oap_accesstoken")]
        public ActionResult GetAccessToken(int client_id, string client_secret, string grant_type)
        {
            return Invoke(() =>
            {
                CheckOAuthParam(client_id, client_secret, grant_type);

                //TODO: 这里去调用适当的方法去获取系统内用户的ID
                var userId = 10000;
                var accessToken = AccessTokenService.CreateAccessToken(userId, client_id);
                HttpContext.Items["userId"] = userId;

                return new
                {
                    access_token = accessToken.Token,
                    expires_in = accessToken.Expire,
                    userId = userId
                };
            });
        }

        [ActionName("renewal")]
        public ActionResult RenewalAccessToken(string accessToken)
        {
            return Invoke(() =>
            {
                AccessTokenService.UpdateIssueDate(accessToken);
                return new { Result = true };
            });
        }

        [ActionName("signout")]
        public ActionResult Logout(string accessToken)
        {
            return Invoke(() =>
            {
                AccessTokenService.Logout(accessToken);
                return new { Result = true };
            });
        }

        /// <summary>
        /// 检查oauth2相关参数
        /// </summary>
        /// <param name="client_id"></param>
        /// <param name="client_secret"></param>
        /// <param name="grant_type"></param>
        private void CheckOAuthParam(int client_id, string client_secret, string grant_type)
        {
            //检查grant_type
            if (grant_type != "password")
                throw new ApiException("手机端只支持password方式授权", ResultCode.GrantType_Error);
            //检查client_id 和client_secret
            var client = OAuthClientService.GetClientBy(client_id, client_secret);
            if (client == null)
                throw new ApiException("客户端id不存在或者client_secret有误", ResultCode.Client_Error);
        }
    }
}
