using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Projects.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// 规约扩展
    /// </summary>
    public static class SpecificationExtend
    {
        /// <summary>
        /// 定义规约的分区分表参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="shardParams"></param>
        /// <returns></returns>
        public static ISpecification<T> Shard<T>(this ISpecification<T> spec, ShardParams shardParams)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            return spec.Provider.Shard(spec, shardParams);
        }

        /// <summary>
        /// 定义规约的分区分表参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="shardParam"></param>
        /// <returns></returns>
        public static ISpecification<T> Shard<T>(this ISpecification<T> spec, long shardParam)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            return spec.Provider.Shard(spec, ShardParams.Form(shardParam));
        }

        /// <summary>
        /// 定义规约的分区分表参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="shardParam"></param>
        /// <returns></returns>
        public static ISpecification<T> Shard<T>(this ISpecification<T> spec, long shardParam1, long shardParam2)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            return spec.Provider.Shard(spec, ShardParams.Form(shardParam1, shardParam2));
        }


        /// <summary>
        /// 定义规约的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static IConditionSpecification<T> Where<T>(this ISpecification<T> spec, Expression<Func<T, bool>> expr)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (expr == null)
                throw new ArgumentNullException("expr");

            // 多个where 使用 And 关系进行组合 HHB 2012-10-12
            if (spec.CriteriaExpression != null)
                expr = spec.CriteriaExpression.And(expr);

            return (IConditionSpecification<T>)spec.Provider.CreateSpecification(spec.ShardParams, expr);
        }

        /// <summary>
        /// 向规约追加“和”的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static IConditionSpecification<T> And<T>(this IConditionSpecification<T> spec, Expression<Func<T, bool>> expr)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (expr == null)
                throw new ArgumentNullException("expr");

            return (IConditionSpecification<T>)spec.Provider.CreateSpecification(spec.ShardParams, spec.CriteriaExpression.And(expr));
        }

        /// <summary>
        /// 向规约追加“或”的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static IConditionSpecification<T> Or<T>(this IConditionSpecification<T> spec, Expression<Func<T, bool>> expr)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (expr == null)
                throw new ArgumentNullException("expr");

            return (IConditionSpecification<T>)spec.Provider.CreateSpecification(spec.ShardParams, spec.CriteriaExpression.Or(expr));
        }

        /// <summary>
        /// 向规约追加“非”的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <returns></returns>
        public static IConditionSpecification<T> Not<T>(this IConditionSpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");

            return (IConditionSpecification<T>)spec.Provider.CreateSpecification(spec.ShardParams, spec.CriteriaExpression.Not());
        }

        /// <summary>
        /// 指定从序列的开头返回指定数量的连续元素的约束
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static ISpecification<T> Take<T>(this ISpecification<T> spec, int take)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (take <= 0)
                throw new ArgumentOutOfRangeException("take");

            return spec.Provider.Take(spec, take);
        }

        /// <summary>
        /// 指定跳过序列中指定数量的元素然后返回剩余的元素的约束
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public static ISpecification<T> Skip<T>(this ISpecification<T> spec, int skip)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (skip < 0) //skip 可以允许为0,modify by skypan 2011年12月2日15:52:46
                throw new ArgumentOutOfRangeException("skip");

            return spec.Provider.Skip(spec, skip);
        }

        /// <summary>
        /// 指定规约根据键按升序对序列的元素排序的规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IOrderedSpecification<T> OrderBy<T, K>(this ISpecification<T> spec, Expression<Func<T, K>> keySelector)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return spec.Provider.OrderBy(spec, keySelector, QueryOrder.Ascending);
        }

        /// <summary>
        /// 指定规约根据键按降序对序列的元素排序的规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IOrderedSpecification<T> OrderByDescending<T, K>(this ISpecification<T> spec, Expression<Func<T, K>> keySelector)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            return spec.Provider.OrderBy(spec, keySelector, QueryOrder.Descending);
        }

        /// <summary>
        /// 指定规约根据某个键按升序序对序列中的元素执行后续排序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IOrderedSpecification<T> ThenBy<T, K>(this IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            return spec.Provider.ThenBy(spec, keySelector, QueryOrder.Ascending);
        }

        /// <summary>
        /// 指定规约根据某个键按降序对序列中的元素执行后续排序。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IOrderedSpecification<T> ThenByDescending<T, K>(this IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            return spec.Provider.ThenBy(spec, keySelector, QueryOrder.Descending);
        }
    }
}
