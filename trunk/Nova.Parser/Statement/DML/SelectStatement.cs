using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SelectStatement : QueryStatement
    {
        public SelectStatement(SelectOption option, IDictionary<IExpression, string> selectExprList,
            TableReferences tables, IExpression where, GroupByFragment group,
            IExpression having, OrderByFragment order, LimitFragment limit)
        {
            if (option == null)
                throw new ArgumentNullException("option");
            if (selectExprList == null || selectExprList.Count == 0)
                this.SelectExpressionList = new Dictionary<IExpression, string>();
            else
                this.SelectExpressionList = selectExprList;

            this.Table = tables;
            this.Option = option;
            this.Where = where;
            this.Order = order;
            this.Having = having;
            this.Group = group;
            this.Limit = limit;
        }

        public GroupByFragment Group { get; protected set; }
        public IExpression Having { get; protected set; }
        public LimitFragment Limit { get; protected set; }
        public OrderByFragment Order { get; protected set; }
        public SelectOption Option { get; protected set; }
        public IExpression Where { get; protected set; }
        public TableReferences Table { get; protected set; }
        public IDictionary<IExpression, string> SelectExpressionList { get; protected set; }


        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<SelectStatement>(this);
        }

        public sealed static class SelectOption
        {
            public SelectDuplicationStrategy resultDup = SelectDuplicationStrategy.All;
            public bool highPriority = false;
            public bool straightJoin = false;
            public SmallOrBigResult resultSize = SmallOrBigResult.Undefine;
            public bool sqlBufferResult = false;
            public QueryCacheStrategy queryCache = QueryCacheStrategy.Undefine;
            public bool sqlCalcFoundRows = false;
            public LockMode lockMode = LockMode.Undefine;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SelectOption").Append('{');
                sb.Append("resultDup").Append('=').Append(resultDup.ToString());
                sb.Append(", ").Append("highPriority").Append('=').Append(highPriority);
                sb.Append(", ").Append("straightJoin").Append('=').Append(straightJoin);
                sb.Append(", ").Append("resultSize").Append('=').Append(resultSize.ToString());
                sb.Append(", ").Append("sqlBufferResult").Append('=').Append(sqlBufferResult);
                sb.Append(", ").Append("queryCache").Append('=').Append(queryCache.ToString());
                sb.Append(", ").Append("sqlCalcFoundRows").Append('=').Append(sqlCalcFoundRows);
                sb.Append(", ").Append("lockMode").Append('=').Append(lockMode.ToString());
                sb.Append('}');
                return sb.ToString();
            }
        }

        public enum SelectDuplicationStrategy
        {
            All,
            Distinct,
            DistinctRow
        }

        public enum QueryCacheStrategy
        {
            Undefine,
            SqlCache,
            SqlNoCache
        }

        public enum SmallOrBigResult
        {
            Undefine,
            SqlSmallResult,
            SqlBigResult
        }

        public enum LockMode
        {
            Undefine,
            ForUpdate,
            LockInShareMode
        }
    }
}
