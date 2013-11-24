using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework.Querys
{
    public interface IQuerySpecification
    {
        Type ElementType { get; }

        Expression Expression { get; }

        IQuerySpecificationProvider Provider { get; }
    }

    public interface IQuerySpecification<T> : IQuerySpecification
    {
    }

    public interface IQueryOrderedSpecification : IQuerySpecification
    {
    }

    public interface IQueryOrderedSpecification<T> : IQuerySpecification<T>, IQueryOrderedSpecification
    {
    }
}
