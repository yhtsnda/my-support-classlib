using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Projects.Access.MongoAccess
{
    public static class MongoQueryable
    {
        private class MongoUpdateable<T> : IMongoUpdateable<T>
        {
            public IQueryable<T> Query;
        }

        /// <summary>
        /// 设置更新的对象属性及其值
        /// </summary>
        /// <typeparam name="TSource">对象的类型</typeparam>
        /// <typeparam name="TValue">更新的对象属性的类型</typeparam>
        /// <param name="source">更新的对象的序列</param>
        /// <param name="extract">属性选择的表达式</param>
        /// <param name="value">更新的值</param>
        /// <returns></returns>
        public static IMongoUpdateable<TSource> Set<TSource, TValue>(this IQueryable<TSource> source, Expression<Func<TSource, TValue>> extract, TValue value)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (extract == null)
                throw new ArgumentNullException("extract");

            IQueryable<TSource> query = source.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TValue) }), new Expression[] { source.Expression, Expression.Quote(extract), Expression.Constant(value, typeof(TValue)) }));
            return new MongoUpdateable<TSource> { Query = query };
        }

        /// <summary>
        /// 设置更新的对象属性及其值
        /// </summary>
        /// <typeparam name="TSource">对象的类型</typeparam>
        /// <typeparam name="TValue">更新的对象属性的类型</typeparam>
        /// <param name="source">更新的对象的序列</param>
        /// <param name="extract">属性选择的表达式</param>
        /// <param name="value">更新的值</param>
        /// <returns></returns>
        public static IMongoUpdateable<TSource> Set<TSource, TValue>(this IMongoUpdateable<TSource> source, Expression<Func<TSource, TValue>> extract, TValue value)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (extract == null)
                throw new ArgumentNullException("extract");

            IQueryable<TSource> query = ((MongoUpdateable<TSource>)source).Query;
            query = query.Provider.CreateQuery<TSource>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource), typeof(TValue) }), new Expression[] { query.Expression, Expression.Quote(extract), Expression.Constant(value, typeof(TValue)) }));
            return new MongoUpdateable<TSource> { Query = query };
        }

        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <typeparam name="TSource">对象的类型</typeparam>
        /// <param name="source">执行更新的序列</param>
        /// <returns></returns>
        public static bool Update<TSource>(this IMongoUpdateable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            IQueryable<TSource> query = ((MongoUpdateable<TSource>)source).Query;
            return query.Provider.Execute<bool>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { query.Expression }));
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <typeparam name="TSource">对象的类型</typeparam>
        /// <param name="source">执行删除的序列</param>
        /// <returns></returns>
        public static bool Delete<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.Execute<bool>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression }));
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <typeparam name="TSource">对象的类型</typeparam>
        /// <param name="source">执行删除的序列</param>
        /// <param name="predicate">删除的条件</param>
        /// <returns></returns>
        public static bool Delete<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Provider.Execute<bool>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(predicate) }));
        }
    }
}
