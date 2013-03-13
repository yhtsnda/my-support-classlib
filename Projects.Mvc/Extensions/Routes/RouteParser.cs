using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Reflection;

namespace Projects.Framework.Web
{
    /// <summary>
    /// Url的Route解析器
    /// </summary>
    public static class RouteParser
    {
        private static MethodInfo _parseInvoker;

        static RouteParser()
        {
            var parserType = typeof(Route).Assembly.GetType("System.Web.Routing.RouteParser");
            _parseInvoker = parserType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
        }

        public static ParsedRoute Parse(string routeUrl)
        {
            return new ParsedRoute(_parseInvoker.Invoke(null, new object[] { routeUrl }));
        }
    }

    public class ParsedRoute
    {
        private static MethodInfo _matchInvoker;
        private object _instance;

        static ParsedRoute()
        {
            var routeType = typeof(Route).Assembly.GetType("System.Web.Routing.ParsedRoute");
            _matchInvoker = routeType.GetMethod("Match", BindingFlags.Instance | BindingFlags.Public);
        }

        public ParsedRoute(object instance)
        {
            _instance = instance;
        }

        public RouteValueDictionary Match(string virtualPath, RouteValueDictionary defaultValues)
        {
            return (RouteValueDictionary)_matchInvoker.Invoke(_instance, new object[] { virtualPath, defaultValues });
        }
    }
}
