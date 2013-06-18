using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Projects.OAuth
{
    /// <summary>
    /// OAuth认证抽象基类
    /// </summary>
    public class AbstractOAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var oauthService = Projects.Framework.DependencyResolver.Resolve<OAuthService>();
            var accessGrant = OAuthAuthorization.ValidToken();
            OnValidateSuccess(filterContext, accessGrant);
        }

        protected virtual void OnValidateSuccess(AuthorizationContext filterContext, ServerAccessGrant accessGrant)
        {
        }
    }
}
