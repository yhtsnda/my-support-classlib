using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using Avalon.Utility;

namespace Avalon.Framework
{
    internal class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        public string CreateCacheKey(MethodBase method, object[] inputs)
        {
            var sb = new StringBuilder();

            if (method.DeclaringType != null)
            {
                sb.Append(method.DeclaringType.ToPrettyString());
            }
            sb.Append('.');
            sb.Append(method.Name);

            if (inputs != null)
            {
                foreach (var input in inputs)
                {
                    sb.Append(":");
                    if (input != null)
                        SimpleGenerator.Serialize(input, sb);
                }
            }
            return sb.ToString();
        }
    }
}
