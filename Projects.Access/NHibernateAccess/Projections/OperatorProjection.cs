using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace Projects.Framework.NHibernateAccess.Projections
{
    /// <summary>
    /// from http://savale.blogspot.com/2011/04/nhibernate-and-missing.html
    /// </summary>
    public abstract class OperatorProjection : SimpleProjection
    {
        /// <summary>
        /// args
        /// </summary>
        private readonly IProjection[] args;

        /// <summary>
        /// 返回类型
        /// </summary>
        private readonly IType returnType;

        /// <summary>
        /// op
        /// </summary>
        private string op;

        /// <summary>
        /// OperatorProjection
        /// </summary>
        /// <param name="op"></param>
        /// <param name="returnType"></param>
        /// <param name="args"></param>
        protected OperatorProjection(string op, IType returnType, params IProjection[] args)
        {
            this.Op = op;
            this.returnType = returnType;
            this.args = args;
        }

        /// <summary>
        /// AllowedOperators
        /// </summary>
        public abstract string[] AllowedOperators { get; }

        /// <summary>
        /// IsAggregate
        /// </summary>
        public override bool IsAggregate
        {
            get { return false; }
        }

        /// <summary>
        /// IsGrouped
        /// </summary>
        public override bool IsGrouped
        {
            get
            {
                foreach (IProjection projection in args)
                {
                    if (projection.IsGrouped)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Op
        /// </summary>
        private string Op
        {
            get
            {
                return op;
            }

            set
            {
                var trimmed = value.Trim();
                if (System.Array.IndexOf(AllowedOperators, trimmed) == -1)
                    throw new ArgumentOutOfRangeException("value", trimmed, "Not allowed operator");
                op = " " + trimmed + " ";
            }
        }

        /// <summary>
        /// ToSqlString
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="position"></param>
        /// <param name="criteriaQuery"></param>
        /// <param name="enabledFilters"></param>
        /// <returns></returns>
        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            SqlStringBuilder sb = new SqlStringBuilder();
            sb.Add("(");

            for (int i = 0; i < args.Length; i++)
            {
                int loc = (position + 1) * 1000 + i;
                SqlString projectArg = GetProjectionArgument(criteriaQuery, criteria, args[i], loc, enabledFilters);
                sb.Add(projectArg);

                if (i < args.Length - 1)
                    sb.Add(Op);
            }

            sb.Add(")");
            sb.Add(" as ");
            sb.Add(GetColumnAliases(position)[0]);
            return sb.ToSqlString();
        }

        /// <summary>
        /// GetTypes
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <returns></returns>
        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new IType[] { returnType };
        }

        /// <summary>
        /// ToGroupSqlString
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criteriaQuery"></param>
        /// <param name="enabledFilters"></param>
        /// <returns></returns>
        public override SqlString ToGroupSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            SqlStringBuilder buf = new SqlStringBuilder();
            foreach (IProjection projection in args)
            {
                if (projection.IsGrouped)
                {
                    buf.Add(projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters)).Add(", ");
                }
            }

            if (buf.Count >= 2)
            {
                buf.RemoveAt(buf.Count - 1);
            }

            return buf.ToSqlString();
        }

        /// <summary>
        /// GetProjectionArgument
        /// </summary>
        /// <param name="criteriaQuery"></param>
        /// <param name="criteria"></param>
        /// <param name="projection"></param>
        /// <param name="loc"></param>
        /// <param name="enabledFilters"></param>
        /// <returns></returns>
        private static SqlString GetProjectionArgument(
            ICriteriaQuery criteriaQuery,
            ICriteria criteria,
            IProjection projection,
            int loc,
            IDictionary<string, IFilter> enabledFilters)
        {
            SqlString sql = projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters);
            return StringHelper.RemoveAsAliasesFromSql(sql);
        }
    }
}
