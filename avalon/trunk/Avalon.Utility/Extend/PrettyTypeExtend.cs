using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public static class PrettyTypeExtend
    {
        public static string ToPrettyString(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            StringBuilder sb = new StringBuilder();
            var gtype = type.GetGenericTypeDefinition();
            sb.Append(gtype.Name.Remove(gtype.Name.IndexOf("`")) + "<");

            bool flag = false;
            foreach (var stype in type.GetGenericArguments())
            {
                if (flag)
                    sb.Append(",");
                sb.Append(ToPrettyString(stype));
                flag = true;
            }
            sb.Append(">");
            return sb.ToString(); ;
        }

        public static string Ellipsis(this string value, int length)
        {
            if (String.IsNullOrEmpty(value) || value.Length <= length)
                return value;
            return value.Substring(0, length) + "...";
        }
    }
}
