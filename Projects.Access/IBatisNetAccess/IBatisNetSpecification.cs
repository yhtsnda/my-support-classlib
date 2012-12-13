using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Tool;
using Projects.Accesses;
using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Accesses.IBatisNetAccess
{
    /// <summary>
    /// IBatis规约实现
    /// </summary>
    public class IBatisSpecification<T> : 
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
            get { throw new NotImplementedException(); }
        }

        public ShardParams ShardParams
        {
            get { throw new NotImplementedException(); }
        }

        public ISpecificationProvider Provider
        {
            get { throw new NotImplementedException(); }
        }

        internal IBatisSpecification(ISpecificationProvider provider)
        {
            this.mProvider = provider;
        }
    }
}
