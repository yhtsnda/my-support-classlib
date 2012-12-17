using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Framework.Shards;

namespace Projects.Framework.Specification
{
    public class IBatisNetSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>()
        {
            return new IBatisNetSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(ShardParams sharedParams, 
            Expression<Func<T, bool>> exp)
        {
            return new IBatisNetSpecification<T>(this, sharedParams, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, 
            Expression<Func<T, K>> keySelector, QueryOrder order)
        {

            IBatisNetSpecification<T> batisSpec = (IBatisNetSpecification<T>)spec;
            if (order == QueryOrder.Ascending)
                batisSpec.Query = batisSpec.Query.OrderBy(keySelector);
            else
                batisSpec.Query = batisSpec.Query.OrderByDescending(keySelector);
            return batisSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, 
            Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            IBatisNetSpecification<T> batisSpec = (IBatisNetSpecification<T>)spec;

            if (order == QueryOrder.Ascending)
                batisSpec.Query = ((IOrderedQueryable<T>) batisSpec.Query).ThenBy(keySelector);
            else
                batisSpec.Query = ((IOrderedQueryable<T>)batisSpec.Query).ThenByDescending(keySelector);
            return batisSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            IBatisNetSpecification<T> batisSpec = (IBatisNetSpecification<T>)spec;
            batisSpec.Query = batisSpec.Query.Take(count);
            return batisSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            IBatisNetSpecification<T> batisSpec = (IBatisNetSpecification<T>)spec;
            batisSpec.Query = batisSpec.Query.Skip(count);
            return batisSpec;
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams)
        {
            IBatisNetSpecification<T> batisSpec = (IBatisNetSpecification<T>)spec;
            spec.Shard(shardParams);
            return batisSpec;
        }
    }
}
