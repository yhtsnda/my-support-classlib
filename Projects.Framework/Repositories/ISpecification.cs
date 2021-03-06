﻿using System;
using System.Linq.Expressions;
using Projects.Tool;

namespace Projects.Framework
{
    /// <summary>
    /// 规约接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// 获取规约中的条件表达式
        /// </summary>
        Expression<Func<T, bool>> CriteriaExpression { get; }

        /// <summary>
        /// 获取分区相关的参数信息
        /// </summary>
        ShardParams ShardParams { get; }

        /// <summary>
        /// 获取规约提供者对象
        /// </summary>
        ISpecificationProvider Provider { get; }
    }

    /// <summary>
    /// 已排序的实体规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOrderedSpecification<T> : ISpecification<T>
    {
    }

    /// <summary>
    /// 条件规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConditionSpecification<T> : ISpecification<T>
    {
    }
}
