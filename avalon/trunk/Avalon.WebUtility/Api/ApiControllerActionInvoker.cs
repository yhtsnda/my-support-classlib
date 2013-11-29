using Avalon.Profiler;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Avalon.WebUtility
{
    public class ApiControllerActionInvoker : ControllerActionInvoker
    {
        internal const string ActionResultInvoker = "__ActionResultInvoker__";
        const string StatOpenApi = "openapi";

        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            //open api counter 2013-10-12 hhb
            if (actionDescriptor.IsDefined(typeof(OpenApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(OpenApiAttribute), true))
            {
                if (StatService.CheckEnabled(StatOpenApi))
                {
                    var key = actionDescriptor.ControllerDescriptor.ControllerName + "." + actionDescriptor.ActionName;
                    StatService.Increment(StatOpenApi, key);
                }
            }

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

            if (!(actionReturnValue is ActionResult))
            {
                if (actionDescriptor.IsDefined(typeof(OpenApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(OpenApiAttribute), true))
                    return new OpenApiDataResult(actionReturnValue);

                if (actionDescriptor.IsDefined(typeof(AjaxApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(AjaxApiAttribute), true))
                    return new AjaxApiDataResult(actionReturnValue);
            }
            return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
        }
    }
}