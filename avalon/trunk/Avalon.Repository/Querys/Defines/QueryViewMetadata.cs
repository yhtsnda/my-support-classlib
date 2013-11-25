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
    /// 查询视图的元数据
    /// </summary>
    internal class QueryViewMetadata
    {
        HashSet<MemberInfo> aliasMembers;

        public QueryViewMetadata(Type viewType, Type entityType, Expression aliasExp)
        {
            Arguments.NotNull(viewType, "viewType");
            Arguments.NotNull(entityType, "entityType");
            Arguments.NotNull(aliasExp, "aliasExp");

            ViewType = viewType;
            EntityType = entityType;
            AliasExpression = aliasExp;
            Joins = new List<QueryViewJoinMetadata>();
        }

        public Type EntityType { get; set; }

        public Expression AliasExpression { get; set; }

        internal List<QueryViewJoinMetadata> Joins { get; set; }

        public Type ViewType { get; set; }

        public void ValidAlias(Expression propertyExp, out Type entityType)
        {
            if (aliasMembers == null)
            {
                var alias = new List<Expression>();
                alias.Add(AliasExpression);
                alias.AddRange(Joins.Select(o => o.AliasExpression));
                aliasMembers = alias.ToHashSet(o => GetAliasMemberFormAlias(o).Member);
            }
            var member = GetAliasMemberFormProperty(propertyExp, out entityType);
            if (!aliasMembers.Contains(member))
                throw new QueryDefineException("给定的表达式 {0} 非别名的属性访问或别名未进行定义", propertyExp);

        }

        MemberInfo GetAliasMemberFormProperty(Expression propertyExp, out Type entityType)
        {
            if (propertyExp.NodeType == ExpressionType.Lambda)
                propertyExp = ((LambdaExpression)propertyExp).Body;

            if (propertyExp.NodeType != ExpressionType.MemberAccess)
                throw new QueryDefineException("给定的表达式 {0} 非属性访问", propertyExp);

            propertyExp = ((MemberExpression)propertyExp).Expression;
            if (propertyExp.NodeType != ExpressionType.MemberAccess)
                throw new QueryDefineException("给定的表达式 {0} 非别名的属性访问", propertyExp);

            var memberExp = GetAliasMemberFormAlias(propertyExp);
            entityType = memberExp.Type;
            return memberExp.Member;
        }

        MemberExpression GetAliasMemberFormAlias(Expression aliasExp)
        {
            if (aliasExp.NodeType == ExpressionType.Lambda)
                aliasExp = ((LambdaExpression)aliasExp).Body;

            return (MemberExpression)aliasExp;
        }
    }
}
