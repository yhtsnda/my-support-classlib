using NHibernate.Criterion.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NHibernate
{
    /// <summary>
    /// QueryOverProjectionBuilderExtend
    /// </summary>
    public static class QueryOverProjectionBuilderExtend
    {
        /// <summary>
        /// SelectEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static QueryOverProjectionBuilder<T> SelectEntity<T>(this QueryOverProjectionBuilder<T> builder) where T : class
        {
            System.Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                    builder.Select(NHibernate.Criterion.Projections.Property(pi.Name).As(pi.Name));
            }

            return builder;
        }
    }
}
