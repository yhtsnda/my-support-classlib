using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public interface IQuerySpecificationProvider
    {
        IQuerySpecification<T> CreateSpecification<T>();

        IQuerySpecification<T> CreateSpecification<T>(Expression expression);

        object Execute(Type queryType, MethodInfo method, Expression expression);

        object ExecuteItems<TItem>(Type queryType, MethodInfo method, Expression expression);
    }
}
