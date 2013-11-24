using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    /// <summary>
    /// Linq to odata string
    /// </summary>
    public class LinqToODataStringVisitor : AbstractExpressionVisitor
    {
        ODataStringBuilder query = new ODataStringBuilder();
        int andAlsoCounter = 0;
        ParameterExpression lambdaParamter;

        public LinqToODataStringVisitor(IQuerySpecification spec)
        {
            Visit(spec.Expression);
        }

        public ODataStringBuilder ODataString
        {
            get { return query; }
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
                query.Filter.Append("(");

            Visit(node.Left);
            string bin = "";
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    bin = "and";
                    break;
                case ExpressionType.OrElse:
                    bin = "or";
                    break;
                case ExpressionType.Equal:
                    bin = "eq";
                    break;
                case ExpressionType.NotEqual:
                    bin = "ne";
                    break;
                case ExpressionType.LessThan:
                    bin = "lt";
                    break;
                case ExpressionType.LessThanOrEqual:
                    bin = "le";
                    break;
                case ExpressionType.GreaterThan:
                    bin = "gt";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    bin = "ge";
                    break;
                default:
                    throw new QueryParseException("不支持的运算符 {0}", node.NodeType);
            }
            query.Filter.Append(" " + bin + " ");
            Visit(node.Right);
            if (flag)
                query.Filter.Append(")");

            if (node.NodeType == ExpressionType.AndAlso)
                andAlsoCounter--;
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            CheckCondition();

            if (node.Expression == lambdaParamter)
            {
                query.Filter.Append(GetColumnName(node.Member));
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
                    query.Top = (int)GetValue(node.Arguments[1]);
                    Visit(node.Arguments[0]);
                    return node;
                case MethodType.Skip:
                    query.Skip = (int)GetValue(node.Arguments[1]);
                    Visit(node.Arguments[0]);
                    return node;

                case MethodType.Where:
                case MethodType.And:
                    var af = query.Filter.Length > 0;
                    if (af)
                        andAlsoCounter++;
                    Visit(node.Arguments[0]);
                    if (af)
                        query.Filter.Append(" and ");
                    StateScope(ParseStatus.Condition, () => Visit(node.Arguments[1]));
                    if (af)
                        andAlsoCounter--;
                    break;
                case MethodType.Or:
                    var of = andAlsoCounter > 0;
                    if (of)
                        query.Filter.Append("(");
                    Visit(node.Arguments[0]);
                    if (query.Filter.Length > 0)
                        query.Filter.Append(" OR ");
                    StateScope(ParseStatus.Condition, () => Visit(node.Arguments[1]));
                    if (of)
                        query.Filter.Append(")");
                    break;
                case MethodType.IsIn:
                case MethodType.IsNotIn:
                    CheckCondition();
                    Visit(node.Arguments[0]);
                    query.Filter.AppendFormat(" {0}in (", method == MethodType.IsNotIn ? "not" : "");
                    var vs = ((IEnumerable)GetValue(node.Arguments[1])).Cast<object>();
                    if (vs == null)
                        throw new ArgumentNullException();

                    query.Filter.Append(String.Join(",", vs.Select(o => o)));
                    query.Filter.Append(")");
                    break;
                case MethodType.IsNull:
                case MethodType.IsNotNull:
                    CheckCondition();
                    Visit(node.Arguments[0]);
                    query.Filter.Append(method == MethodType.IsNull ? " eq null" : " ne null");
                    break;

                case MethodType.IsLike:
                    CheckCondition();
                    LikeMode mode = LikeMode.Anywhere;
                    if (node.Arguments.Count == 3)
                        mode = (LikeMode)GetValue(node.Arguments[2]);
                    var m = mode == LikeMode.Anywhere ? "substringof" : (mode == LikeMode.Start ? "startswith" : "endswith");
                    query.Filter.AppendFormat("{0}({1},'{2}')", m, GetColumnName(((MemberExpression)node.Arguments[0]).Member), (string)GetValue(node.Arguments[1]));
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

                    if (query.Orderby.Length > 0)
                        query.Orderby.Append(",");
                    query.Orderby.AppendFormat("{0} {1}", GetColumnName(property), (method == MethodType.OrderByDescending || method == MethodType.ThenByDescending) ? "desc" : "asc");
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
                    query.Filter.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss}'", v);
                else if (v.GetType().IsValueType)
                    query.Filter.Append(v);
                else if (v is string)
                    query.Filter.AppendFormat("'{0}'", v.ToString().Replace("'", "''"));
                else
                    query.Filter.Append(v);
            }
        }

        string GetColumnName(MemberInfo mi)
        {
            if (!(mi is PropertyInfo))
                throw new QueryParseException("成员 {0} 不是属性。", mi.Name);
            return mi.Name;
        }
    }
}
