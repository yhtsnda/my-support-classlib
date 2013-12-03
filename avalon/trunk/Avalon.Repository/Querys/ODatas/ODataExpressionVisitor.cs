using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    /// <summary>
    /// ODataQuery to ISpecification
    /// </summary>
    public class ODataExpressionVisitor
    {
        ODataQueryData query;
        IQuerySpecificationProvider provider;
        ParameterExpression filterParam;
        Type queryType;

        static MethodInfo isInMethod, isNotInMethod, isLikeMethod, isNullMethod, isNotNullMethod;

        static ODataExpressionVisitor()
        {
            var type = typeof(SpecificationExtend);
            isInMethod = type.GetMethod("IsIn");
            isNotInMethod = type.GetMethod("IsNotIn");
            isNullMethod = type.GetMethod("IsNull");
            isNotNullMethod = type.GetMethod("IsNotNull");
            isLikeMethod = type.GetMethods().First(o => o.Name == "IsLike" && o.GetParameters().Length == 3);
        }

        public ODataExpressionVisitor(ODataQueryData query, IQuerySpecificationProvider provider)
        {
            this.query = query;
            this.provider = provider;
        }

        public IQuerySpecification<TFilter> Process<TFilter>()
        {
            queryType = typeof(TFilter);
            var spec = provider.CreateSpecification<TFilter>();
            Expression main = Expression.Constant(spec);
            main = VisitFilter(main, query.FilterNode);
            main = VisitOrderby(main, query.OrderbyNode);
            main = VisitSkip(main, query.SkipNode);
            main = VisitTop(main, query.TopNode);
            return provider.CreateSpecification<TFilter>(main);
        }

        Expression VisitSkip(Expression expression, SkipExpressionNode exp)
        {
            if (exp != null)
            {
                var method = typeof(QuerySpecificationExtend).GetMethod("Skip").MakeGenericMethod(queryType);
                expression = Expression.Call(null, method, expression, Expression.Constant(query.SkipNode.Value));
            }
            return expression;
        }

        Expression VisitTop(Expression expression, TopExpressionNode exp)
        {
            if (exp != null)
            {
                var method = typeof(QuerySpecificationExtend).GetMethod("Take").MakeGenericMethod(queryType);
                expression = Expression.Call(null, method, expression, Expression.Constant(query.TopNode.Value));
            }
            return expression;
        }

        Expression VisitFilter(Expression expression, FilterExpressionNode exp)
        {
            if (exp != null)
            {
                filterParam = Expression.Parameter(queryType, "o");
                var body = Visit(exp.Body);
                var lambda = Expression.Lambda(body, filterParam);

                var method = typeof(QuerySpecificationExtend).GetMethod("Where").MakeGenericMethod(queryType);
                return Expression.Call(null, method, expression, lambda);
            }
            return expression;
        }

        Expression VisitOrderby(Expression expression, OrderbyExpressionNode exp)
        {
            if (exp != null)
            {
                var propertyDic = queryType.GetProperties().ToDictionary(o => o.Name);
                var flag = false;
                var seType = typeof(QuerySpecificationExtend);
                foreach (var item in query.OrderbyNode.Items)
                {
                    string methodName = null;
                    if (!flag)
                        methodName = item.Type == "asc" ? "OrderBy" : "OrderByDescending";
                    else
                        methodName = item.Type == "asc" ? "ThenBy" : "ThenByDescending";

                    var property = propertyDic[item.PropertyName];
                    var method = typeof(QuerySpecificationExtend).GetMethod(methodName).MakeGenericMethod(queryType, property.PropertyType);
                    var propertyParm = Expression.Parameter(queryType, "o");
                    var propertyExp = Expression.Lambda(Expression.Property(propertyParm, property), propertyParm);
                    expression = Expression.Call(null, method, expression, Expression.Quote(propertyExp));
                    flag = true;
                }
            }
            return expression;
        }

        Type GetType(Expression exp)
        {
            if (exp.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)exp).Type;
            if (exp.NodeType == ExpressionType.MemberAccess)
                return ((MemberExpression)exp).Type;
            return null;
        }

        object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
                return Enum.Parse(type, value.ToString());
            return Convert.ChangeType(value, type);
        }

        Expression VisitBinary(BinaryExpressionNode exp)
        {
            var leftExp = Visit(exp.Left);
            if (exp.Left is InExpressionNode || exp.Left is FunctionExpressionNode)
                return leftExp;

            var rightExp = Visit(exp.Right);

            var leftType = GetType(leftExp);
            var rightType = GetType(rightExp);

            var isLeftNull = IsNullExpression(leftExp);
            var isRightNull = IsNullExpression(rightExp);
            if (leftType != rightType && !isLeftNull && !isRightNull)
            {
                if (leftExp.NodeType == ExpressionType.Constant)
                    leftExp = Expression.Constant(ChangeType(((ConstantExpression)leftExp).Value, rightType));
                else if (rightExp.NodeType == ExpressionType.Constant)
                    rightExp = Expression.Constant(ChangeType(((ConstantExpression)rightExp).Value, leftType));
            }

            switch (exp.NodeType)
            {
                case ExpressionNodeType.And:
                    return Expression.AndAlso(leftExp, rightExp);
                case ExpressionNodeType.Or:
                    return Expression.OrElse(leftExp, rightExp);
                case ExpressionNodeType.Equal:
                    if (isRightNull)
                        return Expression.Call(null, isNullMethod.MakeGenericMethod(leftType), leftExp);
                    if (isLeftNull)
                        return Expression.Call(null, isNullMethod.MakeGenericMethod(rightType), rightExp);
                    return Expression.Equal(leftExp, rightExp);
                case ExpressionNodeType.NotEqual:
                    if (isRightNull)
                        return Expression.Call(null, isNotNullMethod.MakeGenericMethod(leftType), leftExp);
                    if (isLeftNull)
                        return Expression.Call(null, isNotNullMethod.MakeGenericMethod(rightType), rightExp);
                    return Expression.NotEqual(leftExp, rightExp);
                case ExpressionNodeType.LessThan:
                    return Expression.LessThan(leftExp, rightExp);
                case ExpressionNodeType.LessThanOrEqual:
                    return Expression.LessThanOrEqual(leftExp, rightExp);
                case ExpressionNodeType.GreaterThan:
                    return Expression.GreaterThan(leftExp, rightExp);
                case ExpressionNodeType.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(leftExp, rightExp);

            }
            throw new NotSupportedException();
        }

        bool IsNullExpression(Expression expression)
        {
            return expression is ConstantExpression && ((ConstantExpression)expression).Value == null;
        }

        Expression VisitConstant(ConstantExpressionNode exp)
        {
            return Expression.Constant(exp.Value);
        }

        Expression VisitProperty(PropertyExpressionNode exp)
        {
            var propertyName = GetPropertyName(filterParam.Type, exp.PropertyName);
            return Expression.Property(filterParam, propertyName);
        }

        Expression VisitFunction(FunctionExpressionNode exp)
        {
            LikeMode mode = LikeMode.Anywhere;
            switch (exp.FunctionName)
            {
                case "substringof":
                    mode = LikeMode.Anywhere;
                    break;
                case "startswith":
                    mode = LikeMode.Start;
                    break;
                case "endswith":
                    mode = LikeMode.End;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return Expression.Call(null, isLikeMethod, Visit(exp.Arguments[0]), Visit(exp.Arguments[1]), Expression.Constant(mode));
        }

        string GetPropertyName(Type type, string propertyName)
        {
            var properties = type.GetProperties();
            var property = properties.FirstOrDefault(o => o.Name.ToLower() == propertyName.ToLower());
            if (property == null)
                throw new AvalonException("类型 {0} 不存在命名为 {1} 的属性", type.FullName, propertyName);
            return property.Name;
        }

        Expression VisitIn(InExpressionNode exp)
        {
            var propertyName = GetPropertyName(queryType, exp.PropertyName);
            var property = queryType.GetProperty(propertyName);
            var method = (exp.Mode == InMode.In ? isInMethod : isNotInMethod).MakeGenericMethod(property.PropertyType);
            var array = Array.CreateInstance(property.PropertyType, exp.Constants.Length);
            exp.Constants.For((o, i) => array.SetValue(o.Value, i));
            return Expression.Call(null, method, Expression.Property(filterParam, property), Expression.Constant(array));
        }

        Expression VisitNot(NotExpressionNode exp)
        {
            throw new NotImplementedException();
        }

        Expression Visit(ExpressionNode exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionNodeType.Not:
                    return VisitNot((NotExpressionNode)exp);
                case ExpressionNodeType.And:
                case ExpressionNodeType.Or:
                case ExpressionNodeType.Equal:
                case ExpressionNodeType.LessThan:
                case ExpressionNodeType.LessThanOrEqual:
                case ExpressionNodeType.GreaterThan:
                case ExpressionNodeType.GreaterThanOrEqual:
                case ExpressionNodeType.NotEqual:
                    return VisitBinary((BinaryExpressionNode)exp);
                case ExpressionNodeType.Constant:
                    return VisitConstant((ConstantExpressionNode)exp);
                case ExpressionNodeType.Property:
                    return VisitProperty((PropertyExpressionNode)exp);
                case ExpressionNodeType.Function:
                    return VisitFunction((FunctionExpressionNode)exp);
                case ExpressionNodeType.In:
                    return VisitIn((InExpressionNode)exp);
                case ExpressionNodeType.Null:
                    return Expression.Constant(null);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
