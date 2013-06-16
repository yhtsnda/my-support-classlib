using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projects.Framework;

namespace Projects.OAuth
{
    public static class OAuthAuthorization
    {
        private const string ContextAccessGrantKey = "__oauthaccessgrant__";
        private static OAuthService oauthService;

        static OAuthAuthorization()
        {
            oauthService = Projects.Framework.DependencyResolver.Resolve<OAuthService>();
        }

        public static int CurrentAppId
        {
            get
            {
                return CurrentToken.GetOrDefault(o => o.ClientId);
            }
        }

        public static long CurrentUserId
        {
            get
            {
                return CurrentToken.GetOrDefault(o => o.UserId);
            }
        }

        public static bool IsAuthorized
        {
            get
            {
                var token = CurrentToken;
                return token != null;
            }
        }

        public static AccessGrant CurrentToken
        {
            get
            {
                AccessGrant accessGrant;
                TryValidToken(out accessGrant);
                return accessGrant;
            }
        }

        public static ServerAccessGrant ValidToken()
        {
            AccessGrant accessGrant;
            if (!TryValidToken(out accessGrant))
                throw new OAuthException("ticket invalid", "ticket required", 500);

            return accessGrant;
        }

        public static bool TryValidToken(out AccessGrant accessGrant)
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                accessGrant = oauthService.TryGetToken(new HttpContextWrapper(context));
                context.Items[ContextAccessGrantKey] = accessGrant;
                return accessGrant != null;
            }
            accessGrant = null;
            return false;
        }

        public static void AppendRequestData(RequestContext context, string name, object value)
        {
            var request = context.HttpContext.Request;
            if (String.IsNullOrEmpty(request.QueryString[name]))
            {
                if (String.IsNullOrEmpty(request.Form[name]))
                {
                    if (!context.RouteData.Values.ContainsKey(name))
                        context.RouteData.Values.Add(name, value);
                }
            }
        }
    }
}
