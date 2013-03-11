
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    internal class DefaultSetter : ISetter
    {
        PropertyInfo method;
        string propertyName;
        Action<object, object> setter;

        public DefaultSetter(PropertyInfo property)
        {
            this.method = property;
            this.propertyName = property.Name;
            CreateSetter();
        }

        void CreateSetter()
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            var main = Expression.Call(Expression.Convert(instance, method.DeclaringType), method.GetSetMethod(true), Expression.Convert(value, method.PropertyType));

            setter = (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, instance, value).Compile();
        }

        public void Set(object target, object value)
        {
            setter(target, value);
        }

        public string PropertyName
        {
            get { return propertyName; }
        }
    }
}
