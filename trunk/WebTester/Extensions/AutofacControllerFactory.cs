using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Projects.Framework.Web;
using Autofac;

namespace WebTester
{
    public class AutofacControllerFactory : ApiControllerFactory
    {
        private readonly IContainer container;

        public AutofacControllerFactory(IContainer container)
        {
            this.container = container;
        }

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var controller = base.GetControllerType(requestContext, controllerName);
            if (controller == null)
            {
                object x;
                if (this.container.TryResolveNamed(controllerName, typeof(IController), out x))
                    controller = x.GetType();
            }
            return controller;
        }
    }
}
