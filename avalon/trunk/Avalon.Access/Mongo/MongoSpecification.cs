using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.MongoAccess
{
    public class MongoSpecification<T> : AbstractSpecification, ISpecification<T>, IOrderedSpecification<T>, IConditionSpecification<T>
    {
        ISpecificationProvider provider;
        ShardParams shardParams;
        Expression<Func<T, bool>> criteriaExpression;
        int take;
        int skip;
        QueryOrderExpression orderByExpression;
        List<QueryOrderExpression> thenByExpressions;

        IQueryable<T> query;

        internal MongoSpecification(ISpecificationProvider provider)
        {
            this.provider = provider;
            this.thenByExpressions = new List<QueryOrderExpression>();
        }

        internal MongoSpecification(ISpecificationProvider provider, ShardParams shardParams, Expression<Func<T, bool>> expr)
        {
            this.provider = provider;
            this.criteriaExpression = expr;
            this.shardParams = shardParams;
            this.thenByExpressions = new List<QueryOrderExpression>();
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

        public int Skip
        {
            get { return skip; }
            internal set { skip = value; }
        }

        public int Take
        {
            get { return take; }
            internal set { take = value; }
        }

        public QueryOrderExpression OrderByExpression
        {
            get { return orderByExpression; }
            internal set { orderByExpression = value; }
        }

        public IEnumerable<QueryOrderExpression> ThenByExpressions
        {
            get { return thenByExpressions; }
        }

        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ShardParams", ShardParams);
            dic.Add("Query", Query.Expression);
            return dic;
        }

        internal IQueryable<T> GetQuery()
        {
            return new MonogQueryable<T>(new MongoQueryExecutor(GetSession(), shardParams));
        }

        internal IQueryable<T> Query
        {
            get
            {
                if (query == null)
                {
                    query = GetQuery();
                    if (criteriaExpression != null)
                        query = GetQuery().Where(criteriaExpression);
                    if (orderByExpression != null)
                        query = orderByExpression.Process<T>(query);
                    foreach (var thenByExpression in thenByExpressions)
                        query = thenByExpression.Process<T>(query);

                    if (skip > 0)
                        query.Skip(skip);
                    if (take > 0)
                        query.Take(take);
                };
                return query;
            }
            set { query = value; }
        }

        MongoSession GetSession()
        {
            var session = (MongoShardSession<T>)RepositoryFramework.OpenSession<T>(shardParams);
            return session.InnerSession;
        }
    }
}
