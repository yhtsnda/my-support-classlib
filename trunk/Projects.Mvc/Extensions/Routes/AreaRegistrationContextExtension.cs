using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class AreaRegistrationContextExtension
    {
        public static void MapRoute(this AreaRegistrationContext context, string name, Route route)
        {
            route.DataTokens["area"] = context.AreaName;
            var namespaces = route.DataTokens.TryGetValue("Namespaces") as string[];
            bool flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;
            context.Routes.Add(name, route);
        }
    }
}
