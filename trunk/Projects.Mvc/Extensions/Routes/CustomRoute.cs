using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public abstract class CustomRoute : Route
    {
        public CustomRoute(string url, object defaults, object constrations, string[] namespaces)
            : base(url, new MvcRouteHandler())
        {
            Defaults = new RouteValueDictionary(defaults);
            Constraints = new RouteValueDictionary(constrations);
            DataTokens = new RouteValueDictionary();

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                DataTokens["Namespaces"] = namespaces;
            }
        }
    }
}
