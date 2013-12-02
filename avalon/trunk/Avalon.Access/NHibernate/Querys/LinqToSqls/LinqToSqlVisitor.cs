using Avalon.Framework;
using Avalon.Framework.Querys;
using NHibernate.Persister.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// linq to sql
    /// </summary>
    public class LinqToSqlVisitor : AbstractExpressionVisitor
    {
        QueryEntityManager entityManager;
        SqlBuilder sql;

        ParameterExpression lambdaParamter;
        int andAlsoCounter = 0;

        public LinqToSqlVisitor(QueryEntityManager entityManager, Expression expression, SqlMode mode = SqlMode.Query)
        {
            sql = new SqlBuilder(mode);
            this.entityManager = entityManager;

            sql.From.AppendFormat("FROM {0} {1}", entityManager.GetTableName(entityManager.Metadata.EntityType), entityManager.GetAlias(entityManager.Metadata.EntityType));
            Visit(expression);

            if (mode == SqlMode.Query)
            {
                sql.Select.Append("SELECT ");
                if (entityManager.Provider.IsSource(entityManager.ResultType))
                {
                    var persister = GetClassMetadata(entityManager.ResultType);
                    var alias = entityManager.GetAlias(entityManager.ResultType);

                    sql.Select.AppendFormat("{0}.{1} `{2}`", alias, persister.IdentifierColumnNames[0], persister.IdentifierPropertyName);
                    sql.Select.Append(String.Concat(persister.PropertyNames.Select(o => String.Format(",{0}.{1} `{2}`", alias, persister.GetPropertyColumnNames(o)[0], o))));
                }
                else
                {
                    sql.Select.Append(String.Join(",", entityManager.ResultType.GetProperties().Select(o => entityManager.ResultPropertyNameToColumn(o.Name) + " `" + o.Name + "`")));
                }
            }
            else
            {
                sql.Select.Append("SELECT COUNT(*) c");
            }

            var joins = entityManager.GetJoins();
            foreach (var join in joins)
            {
                var joinVisitor = new JoinExpressionVisitor(entityManager);
                joinVisitor.Process(join);
                sql.From.Append(" " + joinVisitor.GetSql());
            }
        }

        AbstractEntityPersister GetClassMetadata(Type entityType)
        {
            var factory = (NHibernateShardSessionFactory)RepositoryFramework.GetSessionFactory(entityType);
            return factory.GetSessionFactory(entityType).GetClassMetadata(entityType) as AbstractEntityPersister;
        }

        public SqlBuilder SqlBuilder
        {
            get { return sql; }
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            lambdaParamter = node.Parameters[0];
            return base.VisitLambda<T>(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            CheckCondition();

            if (node.NodeType == ExpressionType.AndAlso)
                andAlsoCounter++;

            var flag = andAlsoCounter > 0 && node.NodeType == ExpressionType.OrElse;
            if (flag)
                sql.Where.Append("(");

            Visit(node.Left);
            string bin = "";
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    bin = "AND";
                    break;
                case ExpressionType.OrElse:
                    bin = "OR";
                    break;
                case ExpressionType.Equal:
                    bin = "=";
                    break;
                case ExpressionType.NotEqual:
                    bin = "!=";
                    break;
                case ExpressionType.LessThan:
                    bin = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    bin = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    bin = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    bin = ">=";
                    break;
                default:
                    throw new QueryParseException("不支持的运算符 {0}", node.NodeType);
            }
            sql.Where.Append(" " + bin + " ");
            Visit(node.Right);
            if (flag)
                sql.Where.Append(")");

            if (node.NodeType == ExpressionType.AndAlso)
                andAlsoCounter--;
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            CheckCondition();

            if (node.Expression == lambdaParamter)
            {
                sql.Where.Append(GetColumnName(node.Member));
            }
            else
            {
                ProcessValue(Expression.Lambda(node).Compile().DynamicInvoke());
                return node;
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            MethodType method = Parse(node.Method);

            switch (method)
            {
                case MethodType.Take:
                    sql.Take = (int)GetValue(node.Arguments[1]);
                    Visit(node.Arguments[0]);
                    return node;
                case MethodType.Skip:
                    sql.Skip = (int)GetValue(node.Arguments[1]);
                    Visit(node.Arguments[0]);
                    return node;

                case MethodType.Where:
                case MethodType.And:
                    var af = sql.Where.Length > 0;
                    if (af)
                        andAlsoCounter++;
                    Visit(node.Arguments[0]);
                    if (sql.Where.Length > 0)
                        sql.Where.Append(" AND ");
                    StateScope(ParseStatus.Condition, () => Visit(node.Arguments[1]));
                    andAlsoCounter--;
                    break;
                case MethodType.Or:
                    var of = andAlsoCounter > 0;
                    if (of)
                        sql.Where.Append("(");
                    Visit(node.Arguments[0]);
                    if (sql.Where.Length > 0)
                        sql.Where.Append(" OR ");
                    StateScope(ParseStatus.Condition, () => Visit(node.Arguments[1]));
                    if (of)
                        sql.Where.Append(")");
                    break;
                case MethodType.IsIn:
                case MethodType.IsNotIn:
                    CheckCondition();
                    Visit(node.Arguments[0]);
                    sql.Where.AppendFormat(" {0}IN (", method == MethodType.IsNotIn ? "NOT " : "");
                    var vs = ((IEnumerable)GetValue(node.Arguments[1])).Cast<object>();
                    if (vs == null)
                        throw new ArgumentNullException();

                    sql.Where.Append(String.Join(",", vs.Select(o => o)));
                    sql.Where.Append(")");
                    break;
                case MethodType.IsNull:
                case MethodType.IsNotNull:
                    CheckCondition();
                    Visit(node.Arguments[0]);
                    sql.Where.AppendFormat(" IS {0}NULL", method == MethodType.IsNotNull ? "NOT " : "");
                    break;

                case MethodType.IsLike:
                    CheckCondition();
                    Visit(node.Arguments[0]);
                    var format = " LIKE '";
                    LikeMode mode = LikeMode.Anywhere;
                    if (node.Arguments.Count == 3)
                        mode = (LikeMode)GetValue(node.Arguments[2]);
                    if (mode == LikeMode.Anywhere || mode == LikeMode.End)
                        format += "%";
                    format += "{0}";
                    if (mode == LikeMode.Anywhere || mode == LikeMode.Start)
                        format += "%";
                    format += "'";

                    sql.Where.AppendFormat(format, ((string)GetValue(node.Arguments[1]))
                        .Replace("'", "\\1'").Replace("\\", "\\\\").Replace("_", "\\_").Replace("%", "\\%"));
                    break;

                case MethodType.OrderBy:
                case MethodType.OrderByDescending:
                case MethodType.ThenBy:
                case MethodType.ThenByDescending:
                    var body = node.Arguments[1];
                    if (body.NodeType == ExpressionType.Quote)
                        body = ((UnaryExpression)body).Operand;

                    var member = (MemberExpression)((LambdaExpression)body).Body;
                    if (member == null)
                        throw new QueryParseException("排序的内容必须为成员表达式, 当前为 {0}", node.Arguments[1].NodeType);

                    var property = (PropertyInfo)member.Member;
                    if (property == null)
                        throw new QueryParseException("排序的成员标识式必须为属性, 当前为 {0}", member.Member.MemberType);

                    if (sql.OrderBy.Length > 0)
                        sql.OrderBy.Append(",");
                    sql.OrderBy.AppendFormat("{0} {1}", GetColumnName(property), (method == MethodType.OrderByDescending || method == MethodType.ThenByDescending) ? "DESC" : "ASC");
                    Visit(node.Arguments[0]);
                    break;
                case MethodType.Unknown:
                    ProcessValue(GetValue(node));
                    break;
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            ProcessValue(node.Value);
            return base.VisitConstant(node);
        }

        void ProcessValue(object v)
        {
            if (v != null && !(v is IQuerySpecification))
            {
                if (v is DateTime)
                    sql.Where.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss}'", v);
                else if (v.GetType().IsEnum)
                    sql.Where.Append((int)v);
                else if (v.GetType().IsValueType)
                    sql.Where.Append(v);
                else if (v is string)
                    sql.Where.AppendFormat("'{0}'", v.ToString().Replace("'", "''"));
                else
                    sql.Where.Append(v);
            }
        }

        string GetColumnName(MemberInfo mi)
        {
            return entityManager.QueryPropertyToColumnName(mi.Name);
        }

    }
}
