using Avalon.Framework.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.NHibernateAccess
{
    public class JoinExpressionVisitor : ExpressionVisitor
    {
        QueryEntityManager entityManager;
        StringBuilder sql = new StringBuilder();

        public JoinExpressionVisitor(QueryEntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        internal void Process(QueryViewJoinMetadata joinMetadata)
        {
            string join = "JOIN";
            if (joinMetadata.JoinType == JoinType.LeftOuterJoin)
                join = "LEFT JOIN";
            sql.AppendFormat("{0} {1} {2} ON ", join, entityManager.GetTableName(joinMetadata.JoinEntityType), entityManager.GetAlias(joinMetadata.JoinEntityType));
            Visit(joinMetadata.ConditionExpression);
        }

        public string GetSql()
        {
            return sql.ToString();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    sql.Append(" = ");
                    break;
                case ExpressionType.LessThan:
                    sql.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sql.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sql.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sql.Append(" >= ");
                    break;
                case ExpressionType.NotEqual:
                    sql.Append(" != ");
                    break;
                case ExpressionType.AndAlso:
                    sql.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                    sql.Append(" or ");
                    break;
                default:
                    throw new NotSupportedException();
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var v = node.Value;
            if (v != null)
            {
                if (v.GetType().IsValueType)
                    sql.Append(v);
                else if (v is string)
                    sql.AppendFormat("'{0}'", v.ToString().Replace("'", "''"));
                else if (v is DateTime)
                    sql.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss}'", v);
                else if (v is Guid)
                    sql.AppendFormat("'{0}'", v);
                else
                    throw new NotSupportedException();
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var entityType = node.Expression.Type;
            sql.AppendFormat("{0}.{1}", entityManager.GetAlias(entityType), entityManager.GetColumnName(entityType, (PropertyInfo)node.Member));
            return node;
        }
    }
}
