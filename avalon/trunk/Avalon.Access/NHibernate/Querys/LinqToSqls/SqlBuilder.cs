using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class SqlBuilder
    {
        SqlMode sqlMode;
        public SqlBuilder(SqlMode mode)
        {
            sqlMode = mode;
            Where = new StringBuilder();
            OrderBy = new StringBuilder();
            From = new StringBuilder();
            Select = new StringBuilder();
        }

        public StringBuilder From { get; private set; }

        public StringBuilder Where { get; private set; }

        public StringBuilder OrderBy { get; private set; }

        public StringBuilder Select { get; private set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string GetQuerySql()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(Select);
            sql.Append(" " + From);
            if (Where.Length > 0)
                sql.Append(" WHERE " + Where);
            if (sqlMode == SqlMode.Query && OrderBy.Length > 0)
                sql.Append(" ORDER BY " + OrderBy);
            if (sqlMode == SqlMode.Query && (Skip > 0 || Take > 0))
                sql.AppendFormat(" LIMIT {0},{1}", Skip, Take == 0 ? Int32.MaxValue : Take);
            return sql.ToString();
        }

        public override string ToString()
        {
            return GetQuerySql();
        }
    }
}
