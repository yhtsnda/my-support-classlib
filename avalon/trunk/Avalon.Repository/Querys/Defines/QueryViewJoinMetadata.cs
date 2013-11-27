using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework.Querys
{
    /// <summary>
    /// 查询视图对象的 JOIN 元数据
    /// </summary>
    public class QueryViewJoinMetadata
    {
        public QueryViewJoinMetadata(Type joinEntityType, Expression aliasExp, Expression conditionExp, JoinType joinType)
        {
            JoinEntityType = joinEntityType;
            AliasExpression = aliasExp;
            ConditionExpression = conditionExp;
            JoinType = joinType;
            ConditionEntityTypes = new HashSet<Type>();
        }

        /// <summary>
        /// 关联的实体类型
        /// </summary>
        public Type JoinEntityType { get; set; }

        /// <summary>
        /// 关联的实体的别名表达式
        /// </summary>
        public Expression AliasExpression { get; set; }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public Expression ConditionExpression { get; set; }

        /// <summary>
        /// 关联的类型
        /// </summary>
        public JoinType JoinType { get; set; }

        /// <summary>
        /// 条件表达式中使用到的实体类型
        /// </summary>
        public HashSet<Type> ConditionEntityTypes { get; set; }
    }
}
