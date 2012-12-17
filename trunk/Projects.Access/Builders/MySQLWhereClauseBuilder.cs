using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Accesses.Builders
{
    /// <summary>
    /// MySQL 的Where Clause Builder
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class MySQLWhereClauseBuilder<TEntity> : WhereClauseBuilder<TEntity> 
        where TEntity : class, new()
    {
        protected internal override char ParameterChar
        {
            get { return '?'; }
        }

        MySQLWhereClauseBuilder(IStorageMappingResolver<TEntity> mappingResolver)
            : base(mappingResolver)
        {
        }
    }
}
