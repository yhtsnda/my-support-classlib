using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Accesses.Builders
{
    /// <summary>
    /// 通用Where子句建立者(SQL Server等)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class GeneralWhereClauseBuilder<TEntity> 
        : WhereClauseBuilder<TEntity> where TEntity : class, new()
    {
        protected internal override char ParameterChar
        {
            get { return '@'; }
        }

        public GeneralWhereClauseBuilder(IStorageMappingResolver<TEntity> mappingResolver)
            : base(mappingResolver)
        {
        }
    }
}
