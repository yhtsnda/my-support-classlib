using Avalon.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Utility
{
    internal static class ReflectionHelper
    {
        public static PropertyInfo GetProperty(Expression expression)
        {
            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }

            if (memberExpression == null)
            {
                return null;
            }

            return (PropertyInfo)memberExpression.Member;
        }

        public static MethodInfo GetMethod(Expression expression)
        {
            MethodCallExpression methodCallExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression;
                methodCallExpression = body.Operand as MethodCallExpression;
            }
            else if (expression.NodeType == ExpressionType.Call)
            {
                methodCallExpression = expression as MethodCallExpression;
            }

            if (methodCallExpression == null)
            {
                return null;
            }

            return methodCallExpression.Method;
        }

        internal static IList Cast(this IEnumerable<object> items, Type type)
        {
            var listType = typeof(List<>).MakeGenericType(type);
            var list = (IList)FastActivator.Create(listType);
            foreach (var item in items)
                list.Add(item);
            return list;
        }

        internal static Type GetEntityTypeFromRepositoryType(Type repositoryType)
        {
            if (repositoryType.IsGenericType && repositoryType.GetGenericTypeDefinition() == typeof(IRepository<>))
                return repositoryType.GetGenericArguments()[0];

            var interfaceType = repositoryType.GetInterface("IRepository`1", false);
            if (interfaceType == null)
                throw new ArgumentException("拦截的接口不实现 IRepository<T> " + repositoryType.FullName);

            return interfaceType.GetGenericArguments()[0];
        }

        public static Action<object, object[]> CreateSetDatasHandler(Type entityType, IList<PropertyInfo> properties)
        {
            var param1 = Expression.Parameter(typeof(object), "target");
            var param2 = Expression.Parameter(typeof(object[]), "values");

            List<Expression> blocks = new List<Expression>();
            var target = Expression.Variable(entityType, "entity");
            blocks.Add(Expression.Assign(target, Expression.Convert(param1, entityType)));
            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var value = Expression.ArrayAccess(param2, Expression.Constant(i));
                blocks.Add(Expression.Call(target, property.GetSetMethod(true), Expression.Convert(value, property.PropertyType)));
            }
            var main = Expression.Block(new ParameterExpression[] { target }, blocks);
            return (Action<object, object[]>)Expression.Lambda(typeof(Action<object, object[]>), main, param1, param2).Compile();
        }

        public static Func<object, object[]> CreateGetDatasHandler(Type entityType, IList<PropertyInfo> properties)
        {
            var param = Expression.Parameter(typeof(object), "target");

            var target = Expression.Variable(entityType, "entity");
            var values = Expression.Variable(typeof(object[]), "values");

            List<Expression> blocks = new List<Expression>();
            blocks.Add(Expression.Assign(target, Expression.Convert(param, entityType)));
            blocks.Add(Expression.Assign(values, Expression.NewArrayBounds(typeof(object), Expression.Constant(properties.Count))));

            for (var i = 0; i < properties.Count; i++)
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
            return (Func<object, object[]>)Expression.Lambda(typeof(Func<object, object[]>), main, param).Compile();
        }
    }
}
