using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Projects.Framework
{
    internal static class ReflectionHelper
    {
        public static PropertyInfo GetMember(Expression expression)
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
                throw new ArgumentException("Not a member access", "expression");
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
                throw new ArgumentException("Not a method call. " + expression.ToString(), "expression");
            }

            return methodCallExpression.Method;
        }

        internal static IList Cast(this IEnumerable<object> items, Type type)
        {
            var listType = typeof(List<>).MakeGenericType(type);
            var list = (IList)Projects.Tool.Reflection.FastActivator.Create(listType);
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

    }
}
