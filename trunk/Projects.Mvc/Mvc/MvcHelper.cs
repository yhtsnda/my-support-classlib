using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;

namespace Projects.Mvc
{
    public class MvcHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        public static void RedirectRouteData(RouteData routeData)
        {
            var requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
            ControllerBuilder.Current.GetControllerFactory()
                  .CreateController(
                      requestContext,
                      routeData.Values["Controller"].ToString()
                  )
                  .Execute(requestContext);
            HttpContext.Current.Response.End();
        }
    }
}
