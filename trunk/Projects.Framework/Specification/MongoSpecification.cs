using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using Projects.Accesses.MongoAccess;

namespace Projects.Framework
{
    public class MongoSpecification<T> : ISpecification<T>, IOrderedSpecification<T>, IConditionSpecification<T>
    {
        ISpecificationProvider provider;
        Expression<Func<T, bool>> criteriaExpression;
        IQueryable<T> query;

        internal MongoSpecification(ISpecificationProvider provider)
        {
            this.provider = provider;
        }

        internal MongoSpecification(ISpecificationProvider provider, Expression<Func<T, bool>> expr)
        {
            this.provider = provider;
            this.criteriaExpression = expr;
        }

        public Expression<Func<T, bool>> CriteriaExpression
        {
            get { return criteriaExpression; }
        }


        public ISpecificationProvider Provider
        {
            get { return provider; }
        }

        internal IQueryable<T> GetQuery()
        {
            return MongoAccessor.GetQuery<T>();
        }

        public IQueryable<T> Query
        {
            get
            {
                if (query == null)
                {
                    if (criteriaExpression == null)
                        query = GetQuery();
                    else
                        query = GetQuery().Where(criteriaExpression);
                };
                return query;
            }
            set { query = value; }
        }


        public string QueryStatement
        {
            get { return null; }
            set { ;}
        }

        public string QueryNumberStatement
        {
            get { return null; }
            set { ;}
        }

        public object SearchObject
        {
            get { return null; }
            set { ;}
        }

        public int? PageIndex
        {
            get { return null; }
            set { ;}
        }

        public int? PageSize
        {
            get { return null; }
            set { ;}
        }
    }
}
