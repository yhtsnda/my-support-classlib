using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace Warehouse.RecordProcessor
{
    internal static class FastActivator
    {
        static Dictionary<Type, Func<object>> factoryCache = new Dictionary<Type, Func<object>>();

        public static T Create<T>()
        {
            return TypeFactory<T>.Create.Invoke();
        }

        public static object Create(string type)
        {
            return Create(Type.GetType(type));
        }

        public static object Create(string type, params Expression[] arguments)
        {
            return Create(Type.GetType(type), arguments);
        }

        public static object Create(Type type)
        {
            Func<object> f;
            if (!factoryCache.TryGetValue(type, out f))
            {
                lock (factoryCache)
                {
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        factoryCache[type] = f = Expression.Lambda<Func<object>>(Expression.New(type), new ParameterExpression[0]).Compile();
                    }
                }
            }
            return f.Invoke();
        }

        public static object Create(Type type, params Expression[] arguments)
        {
            Func<object> f;
            if (!factoryCache.TryGetValue(type, out f))
            {
                lock (factoryCache)
                {
                    if (!factoryCache.TryGetValue(type, out f))
                    {
                        var constructor = type.GetConstructor(arguments.Select(item=>item.Type).ToArray());
                        factoryCache[type] = f = Expression.Lambda<Func<object>>(
                            Expression.New(constructor, arguments), new ParameterExpression[0])
                            .Compile();
                    }
                }
            }
            return f.Invoke();
        }

        static class TypeFactory<T>
        {
            public static readonly Func<T> Create;

            static TypeFactory()
            {
                FastActivator.TypeFactory<T>.Create = Expression.Lambda<Func<T>>(Expression.New(typeof(T)), new ParameterExpression[0]).Compile();
            }
        }
    }



}
