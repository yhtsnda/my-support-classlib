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
            var joinMetadata = new QueryViewJoinMetadata(typeof(TJoin), aliasExp, conditionExp, joinType);
            Metadata.Joins.Add(joinMetadata);
            ValidCondition(joinMetadata, conditionExp);
            return this;
        }

        void ValidCondition(QueryViewJoinMetadata joinMetadata, Expression exp)
        {
            var visitor = new JoinExpressionValidVisitor(Metadata, joinMetadata);
            visitor.Process(exp);
        }

        class JoinExpressionValidVisitor : ExpressionVisitor
        {
            QueryViewMetadata viewMetadata;
            QueryViewJoinMetadata joinMetadata;

            public JoinExpressionValidVisitor(QueryViewMetadata viewMetadata, QueryViewJoinMetadata joinMetadata)
            {
                this.viewMetadata = viewMetadata;
                this.joinMetadata = joinMetadata;
            }

            public void Process(Expression exp)
            {
                Visit(exp);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                Type entityType;
                viewMetadata.ValidAlias(node, out entityType);
                joinMetadata.ConditionEntityTypes.Add(entityType);
                return node;
            }
        }
    }
}
