using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Avalon.Utility;

namespace Avalon.Framework
{
    /// <summary>
    /// 定义一个规约提供接口
    /// </summary>
    public interface ISpecificationProvider
    {
        /// <summary>
        /// 创建一个空的规约对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ISpecification<T> CreateSpecification<T>();

        /// <summary>
        /// 根据条件表达式创建一个规约对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        ISpecification<T> CreateSpecification<T>(ShardParams shardParams, Expression<Func<T, bool>> exp);

        /// <summary>
        /// 加入排序规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order);

        /// <summary>
        /// 在 OrderBy 后加入排序规则
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="spec"></param>
        /// <param name="keySelector"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order);

        /// <summary>
        /// 从序列的开头返回指定数量的连续元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ISpecification<T> Take<T>(ISpecification<T> spec, int count);

        /// <summary>
        /// 跳过序列中指定数量的元素，然后返回剩余的元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ISpecification<T> Skip<T>(ISpecification<T> spec, int count);

        /// <summary>
        /// 定义分区分表信息参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="spec"></param>
        /// <param name="shardParams"></param>
        /// <returns></returns>
        ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams);
    }
}
