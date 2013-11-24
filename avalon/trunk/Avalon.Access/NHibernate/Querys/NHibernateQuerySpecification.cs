using Avalon.Framework.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class NHibernateQuerySpecification<T> : IQuerySpecification<T>, IQueryOrderedSpecification<T>
    {
        public NHibernateQuerySpecification(IQuerySpecificationProvider provider, Expression expression = null)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (!(provider is NHibernateQuerySpecificationProvider))
                throw new ArgumentNullException("给定的 provider 非 NHibernateQuerySpecificationProvider 类型");

            Provider = provider;
            if (expression == null)
                expression = Expression.Constant(this);
            Expression = expression;
        }


        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get;
            private set;
        }

        public IQuerySpecificationProvider Provider
        {
            get;
            private set;
        }

    }
}
