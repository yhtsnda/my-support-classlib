using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projects.OAuth;
using System.Web.Mvc;

namespace Projects.OpenApi
{
    public class OAuthAuthorizeAttribute : Attribute
    {
        protected override void OnValidSuccess(AuthorizationContext filterContext, 
            ServerAccessGrant accessGrant)
        {
            OAuthAuthorization.AppendRequestData(filterContext.RequestContext,
                "userId", accessGrant.UserId);
            OAuthAuthorization.AppendRequestData(filterContext.RequestContext,
                "appId", accessGrant.ClientId);
        }
    }
}