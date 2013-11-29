using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Avalon.WebUtility
{
    public class ApiControllerFactory : DefaultControllerFactory
    {
        [System.Diagnostics.DebuggerHidden]
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var iController = base.GetControllerInstance(requestContext, controllerType);
            if (typeof(Controller).IsAssignableFrom(controllerType))
            {
                Controller controller = iController as Controller;

                if (controller != null)
                    controller.ActionInvoker = new ApiControllerActionInvoker();
            }
            return iController;
        }
    }
}
