using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework
{
    public class QueryOrderExpression
    {
        static MethodInfo orderByMethod;
        static MethodInfo orderByDescendingMethod;
        static MethodInfo thenByMethod;
        static MethodInfo thenByDescendingMethod;

        static QueryOrderExpression()
        {
            var methods = typeof(Queryable).GetMethods();
            orderByMethod = methods.First(o => o.IsGenericMethod && o.Name == "OrderBy" && o.GetGenericArguments().Length == 2 && o.GetParameters().Length == 2);
            orderByDescendingMethod = methods.First(o => o.IsGenericMethod && o.Name == "OrderByDescending" && o.GetGenericArguments().Length == 2 && o.GetParameters().Length == 2);

            thenByMethod = methods.First(o => o.IsGenericMethod && o.Name == "ThenBy" && o.GetGenericArguments().Length == 2 && o.GetParameters().Length == 2);
            thenByDescendingMethod = methods.First(o => o.IsGenericMethod && o.Name == "ThenByDescending" && o.GetGenericArguments().Length == 2 && o.GetParameters().Length == 2);
        }

        bool IsThenBy { get; set; }

        public Expression Expression { get; private set; }

        public QueryOrder QueryOrder { get; private set; }

        public Type ReturnType;

        public IOrderedQueryable<TSource> Process<TSource>(IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (Expression == null)
                throw new ArgumentNullException("Expression");

            MethodInfo method = null;
            if (!IsThenBy)
                method = QueryOrder == QueryOrder.Ascending ? orderByMethod : orderByDescendingMethod;
            else
                method = QueryOrder == QueryOrder.Ascending ? thenByMethod : thenByDescendingMethod;

            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    method.MakeGenericMethod(new Type[] { typeof(TSource), ReturnType }),
                    new Expression[] { source.Expression, Expression.Quote(Expression) }));
        }

        public static QueryOrderExpression CreateOrderBy<T, K>(Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            return new QueryOrderExpression()
            {
                Expression = keySelector,
                QueryOrder = order,
                ReturnType = typeof(K),
                IsThenBy = false
            };
        }

        public static QueryOrderExpression CreateTheneBy<T, K>(Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            return new QueryOrderExpression()
            {
                Expression = keySelector,
                QueryOrder = order,
                ReturnType = typeof(K),
                IsThenBy = true
            };
        }
    }
}
