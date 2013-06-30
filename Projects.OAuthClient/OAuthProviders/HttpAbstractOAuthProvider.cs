using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

using Projects.Tool;
using Projects.Tool.Http;

namespace Projects.OAuthClient
{
    public abstract class HttpAbstractOAuthProvider : AbstractOAuthProvider
    {
        private const int TimeoutDays = 30;
        private const string ContextAccessGrantKey = "__accessgrant__";

        public override OpenApiResult<AccessGrant> UserAuthorize(SimpleTokenData tokenData)
        {
            if (!HttpContext.Current.IsAvailable())
                throw new OAuthException("该方法仅能用于 HTTP 上下文的环境下。");

            return base.UserAuthorize(tokenData);
        }

        protected override void OnUserAuthorizeSuccess(AccessGrant accessGrant)
        {
            Arguments.NotNull(accessGrant, "accessGrant");

            var context = HttpContext.Current;
            if (context.IsAvailable())
            {
                var ticketExpireTime = NetworkTime.Now.AddDays(TimeoutDays);
                var formsIdentity = context.User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    //续约
                    ticketExpireTime = formsIdentity.Ticket.Expiration;
                }

                var ticket = CreateFormsAuthenticationTicket(accessGrant, ticketExpireTime);
                SetAuthCookie(ticket);

                context.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
                context.Items[ContextAccessGrantKey] = accessGrant;
            }
        }

        protected override AccessGrant GetCurrentUserAccessGrant()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                //必须先授权
                var formsIdentity = context.User.Identity as FormsIdentity;
                if (formsIdentity != null && formsIdentity.IsAuthenticated)
                {
                    //先从上下文取
                    var accessGrant = (AccessGrant)context.Items[ContextAccessGrantKey];
                    if (accessGrant == null)
                    {
                        //从UserData解析
                        accessGrant = ParseAccessGrant(formsIdentity.Ticket);
                    }
                    return accessGrant;
                }
            }
            return null;
        }

        protected virtual FormsAuthenticationTicket CreateFormsAuthenticationTicket(AccessGrant accessGrant, DateTime expireTime)
        {
            return new FormsAuthenticationTicket(1, accessGrant.UserId.ToString(), NetworkTime.Now, expireTime, true, AccessGrantSerializer.Serialize(accessGrant));
        }

        void SetAuthCookie(FormsAuthenticationTicket ticket)
        {
            var encryptTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath,
                Secure = FormsAuthentication.RequireSSL
            };
            if (FormsAuthentication.CookieDomain != null)
                cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Expires = ticket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        AccessGrant ParseAccessGrant(FormsAuthenticationTicket ticket)
        {
            var userData = ticket.UserData;
            if (!String.IsNullOrEmpty(userData))
            {
                try
                {
                    return AccessGrantSerializer.Deserialize(userData);
                }
                catch { }
            }
            return null;
        }
    }
}
