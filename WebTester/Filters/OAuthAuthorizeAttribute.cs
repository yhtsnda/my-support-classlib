using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Projects.Framework.OAuth2;

using WebTester.Extensions;
using System.Security.Principal;

namespace WebTester.Filters
{
    /// <summary>
    /// OAuth验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class OAuthAuthorizeAttribute : AuthorizeAttribute
    {
        private static readonly string mArgumentName = "accessToken";

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var accessToken = String.Empty;
            if (httpContext.Request.QueryString.AllKeys.Contains(mArgumentName))
                accessToken = httpContext.Request.QueryString[mArgumentName];
            else if (httpContext.Request.Form.AllKeys.Contains(mArgumentName))
                accessToken = httpContext.Request.Form[mArgumentName];

            var userId = 0;
            try
            {
                userId = AccessTokenService.GetUserIdByAccessToken(accessToken);
            }
            catch (Exception ex)
            {
                var resultCode = ResultCode.ServerError;
                if (ex is ApiException)
                    resultCode = ((ApiException)ex).ResultCode;
                TokenHelper.ResponseError(httpContext, ex.Message, (int)resultCode);
                httpContext.Response.End();
            }

            if (userId == 0) return false;

            var ticket = new FormsAuthenticationTicket(userId.ToString(), true, Int32.MaxValue);
            var formsIdentity = new FormsIdentity(ticket);
            var principal = new GenericPrincipal(formsIdentity, new[] { "Basic" });
            httpContext.User = principal;
            return true;
        }
    }
}