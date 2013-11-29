using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Avalon.OAuth
{
    public class AbstractOAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var oauthService = Avalon.Framework.DependencyResolver.Resolve<OAuthService>();
            var accessGrant = OAuthAuthorization.ValidToken(filterContext.HttpContext);
            OnValidSuccess(filterContext, accessGrant);
        }

        protected virtual void OnValidSuccess(AuthorizationContext filterContext, AccessGrant accessGrant)
        {

        }
    }
}
