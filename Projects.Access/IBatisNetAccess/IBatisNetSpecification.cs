using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Tool;
using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Framework.Specification
{
    /// <summary>
    /// IBatis规约实现
    /// </summary>
    public class IBatisNetSpecification<T> : 
        ISpecification<T>, 
        IOrderedSpecification<T>, 
        IConditionSpecification<T>
    {
        ISpecificationProvider mProvider;
        ShardParams mShardParams;
        Expression<Func<T, bool>> mCriteriaExpression;
        IQueryable<T> query;
        private string mStatementName;
        private int mTake = -1;
        private int mSkip = -1;

        internal IBatisNetSpecification(ISpecificationProvider provider)
        {
            this.mProvider = provider;
        }

        internal IBatisNetSpecification(ISpecificationProvider provider, ShardParams shardParams,
            Expression<Func<T, bool>> expr)
        {
            this.mProvider = provider;
            this.mShardParams = shardParams;
            this.mCriteriaExpression = expr;
        }

        /// <summary>
        /// 规约表达式
        /// </summary>
        public Expression<Func<T, bool>> CriteriaExpression
        {
            get { return mCriteriaExpression; }
        }

        /// <summary>
        /// 规约适配器
        /// </summary>
        public ISpecificationProvider Provider
        {
            get { return mProvider; }
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        public string StatementName
        {
            get { return mStatementName; }
            set { mStatementName = value; }
        }

        /// <summary>
        /// 获取数据的数量
        /// </summary>
        public int Take 
        { 
            get { return mTake; } 
            set { mTake = value; } 
        }

        /// <summary>
        /// 跳过的数量
        /// </summary>
        public int Skip 
        {
            get { return mSkip; } 
            set { mSkip = value; } 
        }


        //将IBatis查询结果转化为Linq对象
        public IQueryable<T> Query
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public ShardParams ShardParams
        {
            get { return this.mShardParams; }
        }
    }
}
