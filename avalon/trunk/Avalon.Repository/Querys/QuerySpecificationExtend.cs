using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public static class QuerySpecificationExtend
    {
        public static IQuerySpecification<TSource> Where<TSource>(this IQuerySpecification<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(predicate) });
            return source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQuerySpecification<TSource> And<TSource>(this IQuerySpecification<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(predicate) });
            return source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQuerySpecification<TSource> Or<TSource>(this IQuerySpecification<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(predicate) });
            return source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQuerySpecification<TSource> Take<TSource>(this IQuerySpecification<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(count) });
            return source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQuerySpecification<TSource> Skip<TSource>(this IQuerySpecification<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Constant(count) });
            return source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQueryOrderedSpecification<TSource> OrderBy<TSource, TKey>(this IQuerySpecification<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TKey) }), new Expression[] { source.Expression, Expression.Quote(keySelector) });
            return (IQueryOrderedSpecification<TSource>)source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQueryOrderedSpecification<TSource> OrderByDescending<TSource, TKey>(this IQuerySpecification<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TKey) }), new Expression[] { source.Expression, Expression.Quote(keySelector) });
            return (IQueryOrderedSpecification<TSource>)source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQueryOrderedSpecification<TSource> ThenBy<TSource, TKey>(this IQueryOrderedSpecification<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TKey) }), new Expression[] { source.Expression, Expression.Quote(keySelector) });
            return (IQueryOrderedSpecification<TSource>)source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IQueryOrderedSpecification<TSource> ThenByDescending<TSource, TKey>(this IQueryOrderedSpecification<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TKey) }), new Expression[] { source.Expression, Expression.Quote(keySelector) });
            return (IQueryOrderedSpecification<TSource>)source.Provider.CreateSpecification<TSource>(expression);
        }

        public static IList<TResult> ToList<TResult>(this IQuerySpecification source)
        {
            var result = (IList<TResult>)source.Provider.ExecuteItems<TResult>(source.ElementType, (MethodInfo)MethodBase.GetCurrentMethod(), source.Expression);
            ProxyProvider.Proxy(result);
            return result;
        }

        public static int Count(this IQuerySpecification source)
        {
            return (int)source.Provider.Execute(source.ElementType, (MethodInfo)MethodBase.GetCurrentMethod(), source.Expression);
        }

        public static PagingResult<TResult> ToPaging<TResult>(this IQuerySpecification source)
        {
            var result = (PagingResult<TResult>)source.Provider.ExecuteItems<TResult>(source.ElementType, (MethodInfo)MethodBase.GetCurrentMethod(), source.Expression);
            ProxyProvider.Proxy(result);
            return result;
        }

    }


}
