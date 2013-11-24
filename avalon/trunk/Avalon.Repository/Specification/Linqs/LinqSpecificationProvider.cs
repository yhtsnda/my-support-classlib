using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework
{
    public class LinqSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>()
        {
            return new LinqSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(ShardParams shardParams, Expression<Func<T, bool>> exp)
        {
            return new LinqSpecification<T>(this, shardParams, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            LinqSpecification<T> linqSpec = (LinqSpecification<T>)spec;
            linqSpec.OrderByExpression = QueryOrderExpression.CreateOrderBy(keySelector, order);
            return linqSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            LinqSpecification<T> linqSpec = (LinqSpecification<T>)spec;
            ((IList<QueryOrderExpression>)linqSpec.ThenByExpressions).Add(QueryOrderExpression.CreateTheneBy(keySelector, order));
            return linqSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            LinqSpecification<T> linqSpec = (LinqSpecification<T>)spec;
            linqSpec.Take = count;
            return linqSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            LinqSpecification<T> linqSpec = (LinqSpecification<T>)spec;
            linqSpec.Skip = count;
            return linqSpec;
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams)
        {
            LinqSpecification<T> linqSpec = (LinqSpecification<T>)spec;
            linqSpec.ShardParams = shardParams;
            return linqSpec;
        }
    }
}
