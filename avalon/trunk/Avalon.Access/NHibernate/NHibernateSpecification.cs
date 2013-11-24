using Avalon.Utility;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Linq;
using Avalon.Framework;

namespace Avalon.NHibernateAccess
{
    public class NHibernateSpecification<T> : AbstractSpecification, ISpecification<T>, IOrderedSpecification<T>, IConditionSpecification<T>
    {
        ISpecificationProvider provider;
        ShardParams shardParams;
        Expression<Func<T, bool>> criteriaExpression;
        IQueryable<T> query;

        internal NHibernateSpecification(ISpecificationProvider provider)
        {
            this.provider = provider;
        }

        internal NHibernateSpecification(ISpecificationProvider provider, ShardParams shardParams, Expression<Func<T, bool>> expr)
        {
            this.provider = provider;
            this.criteriaExpression = expr;
            this.shardParams = shardParams;
        }

        public Expression<Func<T, bool>> CriteriaExpression
        {
            get { return criteriaExpression; }
        }

        public ShardParams ShardParams
        {
            get { return shardParams; }
            internal set { shardParams = value; }
        }

        public ISpecificationProvider Provider
        {
            get { return provider; }
        }

        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ShardParams", ShardParams);
            dic.Add("Query", Query.Expression);
            return dic;
        }

        internal IQueryable<T> Query
        {
            get
            {
                if (query == null)
                {
                    if (criteriaExpression == null)
                        query = GetSession().Query<T>();
                    else
                        query = GetSession().Query<T>().Where(criteriaExpression);
                }
                return query;
            }
            set { query = value; }
        }

        internal ISession GetSession()
        {
            var session = (NHibernateShardSession<T>)RepositoryFramework.OpenSession<T>(shardParams);
            return session.InnerSession;
        }
    }
}
