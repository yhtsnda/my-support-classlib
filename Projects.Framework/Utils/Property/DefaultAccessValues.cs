using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    public class DefaultAccessValues : IAccessValues
    {
        Func<object, object[]> getters;
        Action<object, object[]> setters;

        public DefaultAccessValues(Type entityType)
        {
            var properties = entityType.GetProperties().Where(o => o.CanRead && o.CanWrite).ToArray();

            CreateGetters(entityType, properties);
            CreateSetters(entityType, properties);
        }

        void CreateGetters(Type entityType, PropertyInfo[] properties)
        {
            var param = Expression.Parameter(typeof(object), "target");

            var target = Expression.Variable(entityType, "entity");
            var values = Expression.Variable(typeof(object[]), "values");

            List<Expression> blocks = new List<Expression>();
            blocks.Add(Expression.Assign(target, Expression.Convert(param, entityType)));
            blocks.Add(Expression.Assign(values, Expression.NewArrayBounds(typeof(object), Expression.Constant(properties.Length))));

            for (var i = 0; i < properties.Length; i++)
            {
                var left = Expression.ArrayAccess(values, Expression.Constant(i));
                var right = Expression.Convert(Expression.Property(target, properties[i]), typeof(object));
                blocks.Add(Expression.Assign(left, right));
            }
            LabelTarget returnTarget = Expression.Label(typeof(object[]));
            var returnExpr = Expression.Return(returnTarget, values);

            blocks.Add(returnExpr);
            blocks.Add(Expression.Label(returnTarget, Expression.Constant(new object[0])));

            var main = Expression.Block(new ParameterExpression[] { target, values }, blocks);
            getters = (Func<object, object[]>)Expression.Lambda(typeof(Func<object, object[]>), main, param).Compile();
        }

        void CreateSetters(Type entityType, PropertyInfo[] properties)
        {
            var param1 = Expression.Parameter(typeof(object), "target");
            var param2 = Expression.Parameter(typeof(object[]), "values");

            List<Expression> blocks = new List<Expression>();
            var target = Expression.Variable(entityType, "entity");
            blocks.Add(Expression.Assign(target, Expression.Convert(param1, entityType)));
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var value = Expression.ArrayAccess(param2, Expression.Constant(i));
                blocks.Add(Expression.Call(target, property.GetSetMethod(), Expression.Convert(value, property.PropertyType)));
            }
            var main = Expression.Block(new ParameterExpression[] { target }, blocks);
            setters = (Action<object, object[]>)Expression.Lambda(typeof(Action<object, object[]>), main, param1, param2).Compile();
        }

        public object[] GetPropertyValues(object target)
        {
            return getters(target);
        }

        public void SetPropertyValues(object target, object[] values)
        {
            setters(target, values);
        }


        public System.Collections.IDictionary ToDictionary(object target)
        {
            throw new NotImplementedException();
        }
    }
}
