using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool
{
    public static class Value
    {
        public static V GetOrDefault<T, V>(this T entity, Func<T, V> func) where T : class
        {
            if (entity == null)
                return default(V);
            return func(entity);
        }

        public static V GetOrDefault<T, V>(this T entity, Func<T, V> func, V defaultValue) where T : class
        {
            if (entity == null)
                return defaultValue;
            return func(entity);
        }
    }
}
