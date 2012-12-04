using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    public class MongoSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>(string type = "mongo")
        {
            return new MongoSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(Expression<Func<T, bool>> exp, string type = "mongo")
        {
            return new MongoSpecification<T>(this, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, 
            Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            if (order == QueryOrder.Ascending)
                mongoSpec.Query = mongoSpec.Query.OrderBy(keySelector);
            else
                mongoSpec.Query = mongoSpec.Query.OrderByDescending(keySelector);
            return mongoSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, 
            Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            if (order == QueryOrder.Ascending)
                mongoSpec.Query = ((IOrderedQueryable<T>)mongoSpec.Query).ThenBy(keySelector);
            else
                mongoSpec.Query = ((IOrderedQueryable<T>)mongoSpec.Query).ThenByDescending(keySelector);
            return mongoSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            mongoSpec.Query = mongoSpec.Query.Take(count);
            return mongoSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            MongoSpecification<T> mongoSpec = (MongoSpecification<T>)spec;
            return mongoSpec;
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, Shards.ShardParams shardParams)
        {
            throw new NotImplementedException();
        }
    }
}
