using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class RouteDataExtension
    {
        public static int GetRequiredInt32(this RouteData routeData, string valueName)
        {
            var value = routeData.GetRequiredString(valueName);
            return Int32.Parse(value);
        }

        public static bool TryGetInt32(this RouteData routeData, string valueName, out int value)
        {
            string strValue;
            if (TryGetString(routeData, valueName, out strValue))
            {
                if (Int32.TryParse(strValue, out value))
                    return true;
            }
            value = 0;
            return false;
        }

        public static bool TryGetString(this RouteData routeData, string valueName, out string value)
        {
            object obj2;
            if (routeData.Values.TryGetValue(valueName, out obj2))
            {
                string str = obj2 as string;
                if (!string.IsNullOrEmpty(str))
                {
                    value = str;
                    return true;
                }
            }
            value = null;
            return false;
        }
    }
}
