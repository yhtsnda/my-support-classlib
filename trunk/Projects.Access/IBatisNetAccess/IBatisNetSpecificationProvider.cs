using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Framework.Shards;
using Projects.Framework.Specification;

namespace Projects.Accesses.IBatisNetAccess
{
    public class IBatisNetSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>()
        {
            return new IBatisSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(ShardParams shardParams, Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            throw new NotImplementedException();
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            throw new NotImplementedException();
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            throw new NotImplementedException();
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            throw new NotImplementedException();
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams)
        {
            throw new NotImplementedException();
        }
    }
}
