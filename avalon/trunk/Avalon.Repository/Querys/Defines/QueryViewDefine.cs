using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class QueryViewDefine
    {
        internal QueryViewMetadata Metadata
        {
            get;
            set;
        }

        public QueryViewDefine Join<TJoin>(Expression<Func<TJoin>> aliasExp, Expression<Func<bool>> conditionExp, JoinType joinType = JoinType.OneToOneInnerJoin)
        {
            Metadata.Joins.Add(new QueryViewJoinMetadata(typeof(TJoin), aliasExp, conditionExp, joinType));
            ValidCondition(conditionExp);
            return this;
        }

        void ValidCondition(Expression exp)
        {
            var visitor = new JoinExpressionValidVisitor();
            visitor.Process(exp, Metadata);
        }

        class JoinExpressionValidVisitor : ExpressionVisitor
        {
            QueryViewMetadata viewMetadata;

            public void Process(Expression exp, QueryViewMetadata viewMetadata)
            {
                this.viewMetadata = viewMetadata;
                Visit(exp);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                Type entityType;
                viewMetadata.ValidAlias(node, out entityType);
                return node;
            }
        }
    }
}
