using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace Projects.Accesses.Builders
{
    public interface IWhereClauseBuilder<T> where T : class, new()
    {
        WhereBuilderResult BuildWhereClause(Expression<Func<T, bool>> expression);
    }
}
