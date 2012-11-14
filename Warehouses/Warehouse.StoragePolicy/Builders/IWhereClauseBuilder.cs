using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Warehouse.DataOperator
{
    /// <summary>
    /// 创建基于SQL语句的关系数据库的Where子句
    /// </summary>
    /// <typeparam name="T">需要映射在一个关系性数据库的数据表上的对象</typeparam>
    public interface IWhereClauseBuilder<T> where T : new()
    {
        /// <summary>
        /// 根据表达式创建Where子句
        /// </summary>
        /// <param name="expression">表达式对象</param>
        /// <returns><c>Warehouse.DataOperator.WhereClauseBuildResult</c></returns>
        WhereClauseBuildResult BuildWhereClause(Expression<Func<T, bool>> expression);
    }
}
