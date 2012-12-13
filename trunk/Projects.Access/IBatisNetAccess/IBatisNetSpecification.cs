using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using IBatisNet.DataMapper;

using Projects.Tool;
using Projects.Framework;
using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Accesses.IBatisNetAccess
{
    /// <summary>
    /// IBatis规约实现
    /// </summary>
    public class IBatisNetSpecification<T> : 
        AbstractSpecification,
        ISpecification<T>, 
        IOrderedSpecification<T>, 
        IConditionSpecification<T>
    {
        private ISpecificationProvider mProvider;
        private ShardParams mShardParams;
        private Expression<Func<T,bool>> mCriteriaExpression;
        private int mTake;
        private int mSkip;
        private QueryOrderExpression mOrderByExpression;
        private List<QueryOrderExpression> mThenByExpressions;
        private IQueryable<T> mQuery;

        public Expression<Func<T, bool>> CriteriaExpression
        {
            get { return this.mCriteriaExpression; }
        }

        public ShardParams ShardParams
        {
            get { return this.mShardParams; }
        }

        public ISpecificationProvider Provider
        {
            get { return this.mProvider; }
        }

        public int Skip
        {
            get { return this.mSkip; }
        }

        public int Take
        {
            get { return this.mTake; }
        }

        public QueryOrderExpression OrderByExpression
        {
            get { return this.mOrderByExpression; }
        }

        public IEnumerable<QueryOrderExpression> ThenByExpressions
        {
            get { return this.mThenByExpressions; }
        }

        internal IQueryable<T> Query
        {
            get
            {
                if (mQuery == null)
                {
                    if (mCriteriaExpression == null)
                        mQuery = GetSession().Query<T>();
                    else
                        mQuery = GetSession().Query<T>().Where(mCriteriaExpression);
                }
                return mQuery;
            }
            set { mQuery = value; }
        }

        internal IBatisNetSpecification(ISpecificationProvider provider)
        {
            this.mProvider = provider;
            this.mThenByExpressions = new List<QueryOrderExpression>();
        }

        internal IBatisNetSpecification(ISpecificationProvider provider, ShardParams shardParams,
            Expression<Func<T, bool>> expr)
        {
            this.mProvider = provider;
            this.mCriteriaExpression = expr;
            this.mShardParams = ShardParams;
            this.mThenByExpressions = new List<QueryOrderExpression>();
        }

        internal ISqlMapSession GetSession()
        {
            var session = (IBatisNetShardSession<T>)RepositoryFramework.OpenSession<T>(mShardParams);
            return session.InnerSession;
        }

        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ShardParams", ShardParams);
            dic.Add("Query", Query.Expression);
            return dic;
        }
    }
}
