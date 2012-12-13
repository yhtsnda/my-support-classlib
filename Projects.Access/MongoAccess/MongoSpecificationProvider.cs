using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Accesses.MongoAccess
{
    public class MongoSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>()
        {
            return new MongoSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(ShardParams shardParams, Expression<Func<T, bool>> exp)
        {
            return new MongoSpecification<T>(this, shardParams, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            mongoSpec.OrderByExpression = QueryOrderExpression.CreateOrderBy(keySelector, order);
            return mongoSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            ((IList<QueryOrderExpression>)mongoSpec.ThenByExpressions).Add(QueryOrderExpression.CreateTheneBy(keySelector, order));
            return mongoSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            mongoSpec.Take = count;
            return mongoSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            mongoSpec.Skip = count;
            return mongoSpec;
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            mongoSpec.ShardParams = shardParams;
            return mongoSpec;
        }
    }
}
