using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Projects.Framework
{
    /// <summary>
    /// 通用的规约提供者
    /// </summary>
    public class CommonSpecificationProvider : ISpecificationProvider
    {
        IBatisSpecificationProvider ibatisSpecificationProvider = new IBatisSpecificationProvider();
        MongoSpecificationProvider mongoProvider = new MongoSpecificationProvider();

        public ISpecification<T> CreateSpecification<T>(string type = "ibatis")
        {
            if (type == "mongo")
                return mongoProvider.CreateSpecification<T>();
            else
                return ibatisSpecificationProvider.CreateSpecification<T>();
            
        }

        public ISpecification<T> CreateSpecification<T>(Expression<Func<T, bool>> exp, string type = "ibatis")
        {
            if (type == "mongo")
                return mongoProvider.CreateSpecification<T>(exp);
            return ibatisSpecificationProvider.CreateSpecification<T>(exp);
        }

        public IOrderedSpecification<T> OrderBy<T, K>(ISpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            return spec.Provider.OrderBy(spec, keySelector, order);
        }

        public IOrderedSpecification<T> ThenBy<T, K>(IOrderedSpecification<T> spec, Expression<Func<T, K>> keySelector, QueryOrder order)
        {
            return spec.Provider.ThenBy(spec, keySelector, order);
        }

        public ISpecification<T> Take<T>(ISpecification<T> spec, int count)
        {
            return spec.Provider.Take(spec, count);
        }

        public ISpecification<T> Skip<T>(ISpecification<T> spec, int count)
        {
            return spec.Provider.Skip(spec, count);
        }
    }
}
