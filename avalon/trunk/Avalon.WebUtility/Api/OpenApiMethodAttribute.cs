using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpenApiMethodAttribute : Attribute
    {
        public OpenApiMethodAttribute()
        {
        }

        public OpenApiMethodAttribute(Type requestType)
        {
            RequestTypes = new Type[] { requestType };
        }

        public string Methods { get; set; }

        public Type[] RequestTypes { get; set; }

        public Type[] ResponseTypes { get; set; }

        public string Summary { get; set; }
    }
}
