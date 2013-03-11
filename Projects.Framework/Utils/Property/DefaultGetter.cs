using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public class DefaultGetter : IGetter
    {
        PropertyInfo method;
        string propertyName;
        Func<object, object> getter;

        public DefaultGetter(PropertyInfo method)
        {
            this.method = method;
            this.propertyName = method.Name;
            CreateGetter();
        }

        void CreateGetter()
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var property = Expression.Property(Expression.Convert(instance, method.ReflectedType), method);
            var main = Expression.Convert(property, typeof(object));

            getter = (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, instance).Compile();
        }

        public object Get(object target)
        {
            return getter(target);
        }

        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}
