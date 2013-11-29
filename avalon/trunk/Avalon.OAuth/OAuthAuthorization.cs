using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Avalon.Utility;
using System.Web.Routing;

namespace Avalon.OAuth
{
    public static class OAuthAuthorization
    {
        const string ContextAccessGrantKey = "__oauthaccessgrant__";
        static OAuthService oauthService;
        static OAuthAuthorization()
        {
            oauthService = Avalon.Framework.DependencyResolver.Resolve<OAuthService>();
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

        public static AccessGrant ValidToken()
        {
            return ValidToken(new HttpContextWrapper(HttpContext.Current));
        }

        public static AccessGrant ValidToken(HttpContextBase context)
        {
            var accessGrant = oauthService.TokenValid(context);
            context.Items[ContextAccessGrantKey] = accessGrant;
            return accessGrant;
        }

        public static bool TryValidToken(out AccessGrant accessGrant)
        {
            var context = HttpContext.Current;
            if (context.IsAvailable())
            {
                accessGrant = oauthService.TryGetToken(new HttpContextWrapper(context));
                if (accessGrant != null && !accessGrant.IsExpire())
                {
                    context.Items[ContextAccessGrantKey] = accessGrant;
                    return true;
                }
                accessGrant = null;
                return false;
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
