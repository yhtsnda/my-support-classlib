using System.Web;
using System.Web.Mvc;


namespace Projects.Framework.Web
{
    class ApiControllerActionInvoker : ControllerActionInvoker
    {
        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            if (actionDescriptor is ReflectedActionDescriptor)
            {
                var reflectedActionDescriptor = (ReflectedActionDescriptor)actionDescriptor;
                var returnType = reflectedActionDescriptor.MethodInfo.ReturnType;
                if (returnType == typeof(ActionResult) || returnType.IsSubclassOf(typeof(ActionResult)))
                    return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
            }
            if (actionDescriptor.IsDefined(typeof(OpenApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(OpenApiAttribute), true))
                return new OpenApiDataResult(actionReturnValue);

            if (actionDescriptor.IsDefined(typeof(AjaxApiAttribute), true) || actionDescriptor.ControllerDescriptor.IsDefined(typeof(AjaxApiAttribute), true))
                return new AjaxApiDataResult(actionReturnValue);

            return base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
        }
    }
}
