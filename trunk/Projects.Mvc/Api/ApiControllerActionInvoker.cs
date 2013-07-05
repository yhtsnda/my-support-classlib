using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projects.Framework.Web
{
    public class ApiControllerActionInvoker : ControllerActionInvoker
    {
        internal const string ActionResultInvoker = "__ActionResultInvoker__";


        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            if (actionDescriptor is ReflectedActionDescriptor)
            {
                var reflectedActionDescriptor = (ReflectedActionDescriptor)actionDescriptor;
                var returnType = reflectedActionDescriptor.MethodInfo.ReturnType;
                if (returnType == typeof(ActionResult) || returnType.IsSubclassOf(typeof(ActionResult)))
                {
                    controllerContext.HttpContext.Items[ActionResultInvoker] = true;
                }
            }
            return base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
        }

        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            if (actionDescriptor is ReflectedActionDescriptor)
            {
                var reflectedActionDescriptor = (ReflectedActionDescriptor)actionDescriptor;
                var returnType = reflectedActionDescriptor.MethodInfo.ReturnType;
                if (returnType == typeof(ActionResult) || returnType.IsSubclassOf(typeof(ActionResult)))
                    return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
            }

            if (!(actionReturnValue is OpenApiDataResult) && (actionDescriptor.IsDefined(typeof(OpenApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(OpenApiAttribute), true)))
                return new OpenApiDataResult(actionReturnValue);

            if (!(actionReturnValue is AjaxApiDataResult) && actionDescriptor.IsDefined(typeof(AjaxApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(AjaxApiAttribute), true))
                return new AjaxApiDataResult(actionReturnValue);

            return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
        }
    }
}
