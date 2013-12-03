using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Avalon.Framework.Querys
{
    public abstract class AbstractExpressionVisitor : ExpressionVisitor
    {
        static Dictionary<MethodInfo, MethodType> extendMethods;
        ParseStatus status = ParseStatus.None;

        static AbstractExpressionVisitor()
        {
            extendMethods = typeof(QuerySpecificationExtend).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(o => o.Name != "ToList" && o.Name != "Count" && o.Name != "ToPaging")
                .ToDictionary(o => o, o => (MethodType)Enum.Parse(typeof(MethodType), o.Name));

            extendMethods.Add(typeof(SpecificationExtend).GetMethod("IsIn"), MethodType.IsIn);
            extendMethods.Add(typeof(SpecificationExtend).GetMethod("IsNotIn"), MethodType.IsNotIn);

            var likes = typeof(SpecificationExtend).GetMethods().Where(o => o.Name == "IsLike");
            likes.ForEach(o => extendMethods.Add(o, MethodType.IsLike));
            extendMethods.Add(typeof(SpecificationExtend).GetMethod("IsNull"), MethodType.IsNull);
            extendMethods.Add(typeof(SpecificationExtend).GetMethod("IsNotNull"), MethodType.IsNotNull);
        }

        protected MethodType Parse(MethodInfo method)
        {
            if (method.IsGenericMethod)
                method = method.GetGenericMethodDefinition();

            MethodType type;
            if (extendMethods.TryGetValue(method, out type))
                return type;

            return MethodType.Unknown;
        }

        protected void CheckCondition()
        {
            if (status != ParseStatus.Condition)
                throw new QueryParseException("上下文 {0} 不支持此语法", status);
        }

        protected void StateScope(ParseStatus status, Action action)
        {
            if (this.status != ParseStatus.None)
                throw new QueryParseException("当前状态为 {0} 无法切换到状态 {1}", this.status, status);
            this.status = status;
            action();
            this.status = ParseStatus.None;
        }

        protected object GetValue(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Constant)
                return ((ConstantExpression)expression).Value;

            try
            {
                return Expression.Lambda(expression).Compile().DynamicInvoke();
            }
            catch (Exception ex)
            {
                throw new Exception("无法获取表达式的值。" + expression.ToString(), ex);
            }
        }
    }

    public enum MethodType
    {
        Unknown,
        Where,
        And,
        Or,
        OrderBy,
        OrderByDescending,
        ThenBy,
        ThenByDescending,
        Take,
        Skip,
        IsIn,
        IsNotIn,
        IsLike,
        IsNull,
        IsNotNull
    }

    public  enum ParseStatus
    {
        Condition = 1,
        OrderBy = 2,
        Limit = 3,
        Select = 4,
        None = 99,
    }
}
