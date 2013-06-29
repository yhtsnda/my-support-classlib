using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Tool.Reflection
{
    public static class DelegateAccessor
    {
        public static Func<object, object> CreatePropertyGetter(PropertyInfo property)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expProperty = Expression.Property(Expression.Convert(expInstance, property.DeclaringType), property);
            var main = Expression.Convert(expProperty, typeof(object));

            return (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, expInstance).Compile();
        }

        public static Action<object, object> CreatePropertySetter(PropertyInfo property)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expValue = Expression.Parameter(typeof(object), "value");

            var main = Expression.Call(Expression.Convert(expInstance, property.DeclaringType), property.GetSetMethod(true), Expression.Convert(expValue, property.PropertyType));

            return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expInstance, expValue).Compile();
        }

        public static Func<object, object> CreateFieldGetter(FieldInfo field)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expField = Expression.Field(Expression.Convert(expInstance, field.DeclaringType), field);
            var main = Expression.Convert(expField, typeof(object));

            return (Func<object, object>)Expression.Lambda(typeof(Func<object, object>), main, expInstance).Compile();
        }

        public static Action<object, object> CreateFieldSetter(FieldInfo field)
        {
            var expInstance = Expression.Parameter(typeof(object), "instance");
            var expValue = Expression.Parameter(typeof(object), "value");
            var expField = Expression.Field(Expression.Convert(expInstance, field.DeclaringType), field);

            var main = Expression.Assign(expField, Expression.Convert(expValue, field.FieldType));
            return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expInstance, expValue).Compile();
        }

        public static Action<object, object> CreateFieldCloner(FieldInfo field)
        {
            var expSource = Expression.Parameter(typeof(object), "source");
            var expTarget = Expression.Parameter(typeof(object), "target");

            var expFieldLeft = Expression.Field(Expression.Convert(expTarget, field.DeclaringType), field);
            var expFieldRight = Expression.Field(Expression.Convert(expSource, field.DeclaringType), field);

            var main = Expression.Assign(expFieldLeft, expFieldRight);
            return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expSource, expTarget).Compile();
        }

        public static Action<object, object> CreatePropertyCloner(PropertyInfo property)
        {
            var expSource = Expression.Parameter(typeof(object), "source");
            var expTarget = Expression.Parameter(typeof(object), "target");

            var expPropertySource = Expression.Property(Expression.Convert(expSource, property.DeclaringType), property);

            var main = Expression.Call(Expression.Convert(expTarget, property.DeclaringType), property.GetSetMethod(true), expPropertySource);
            return (Action<object, object>)Expression.Lambda(typeof(Action<object, object>), main, expSource, expTarget).Compile();
        }
    }
}
