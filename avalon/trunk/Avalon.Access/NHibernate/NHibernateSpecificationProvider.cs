using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;
using System.Linq.Expressions;
using Avalon.Framework;

namespace Avalon.NHibernateAccess
{
    public class NHibernateSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>()
        {
            return new NHibernateSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(ShardParams shardParams, Expression<Func<T, bool>> exp)
        {
            return new NHibernateSpecification<T>(this, shardParams, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            NHibernateSpecification<T> nhSpec = (NHibernateSpecification<T>)spec;
            if (order == QueryOrder.Ascending)
                nhSpec.Query = nhSpec.Query.OrderBy(keySelector);
            else
                nhSpec.Query = nhSpec.Query.OrderByDescending(keySelector);
            return nhSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            NHibernateSpecification<T> nhSpec = (NHibernateSpecification<T>)spec;
            if (order == QueryOrder.Ascending)
                nhSpec.Query = ((IOrderedQueryable<T>)nhSpec.Query).ThenBy(keySelector);
            else
                nhSpec.Query = ((IOrderedQueryable<T>)nhSpec.Query).ThenByDescending(keySelector);
            return nhSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            NHibernateSpecification<T> nhSpec = (NHibernateSpecification<T>)spec;
            nhSpec.Query = nhSpec.Query.Take(count);
            return nhSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            NHibernateSpecification<T> nhSpec = (NHibernateSpecification<T>)spec;
            nhSpec.Query = nhSpec.Query.Skip(count);
            return nhSpec;
        }

        public ISpecification<T> Shard<T>(ISpecification<T> spec, ShardParams shardParams)
        {
            NHibernateSpecification<T> nhSpec = (NHibernateSpecification<T>)spec;
            nhSpec.ShardParams = shardParams;
            return nhSpec;
        }
    }
}
