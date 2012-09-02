using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Projects.Framework
{
    public class IBatisSpecificationProvider : ISpecificationProvider
    {
        public ISpecification<T> CreateSpecification<T>(string type = "ibatis")
        {
            return new IBatisSpecification<T>(this);
        }

        public ISpecification<T> CreateSpecification<T>(Expression<Func<T, bool>> exp, string type = "ibatis")
        {
            return new IBatisSpecification<T>(this, exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {

            IBatisSpecification<T> batisSpec = (IBatisSpecification<T>) spec;
            if (order == QueryOrder.Ascending)
                batisSpec.Query = batisSpec.Query.OrderBy(keySelector);
            else
                batisSpec.Query = batisSpec.Query.OrderByDescending(keySelector);
            return batisSpec;
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, 
            Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            IBatisSpecification<T> batisSpec = (IBatisSpecification<T>)spec;

            if (order == QueryOrder.Ascending)
                batisSpec.Query = ((IOrderedQueryable<T>) batisSpec.Query).ThenBy(keySelector);
            else
                batisSpec.Query = ((IOrderedQueryable<T>)batisSpec.Query).ThenByDescending(keySelector);
            return batisSpec;
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            IBatisSpecification<T> batisSpec = (IBatisSpecification<T>)spec;
            batisSpec.Query = batisSpec.Query.Take(count);
            return batisSpec;
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            IBatisSpecification<T> batisSpec = (IBatisSpecification<T>)spec;
            batisSpec.Query = batisSpec.Query.Skip(count);
            return batisSpec;
        }
    }
}
