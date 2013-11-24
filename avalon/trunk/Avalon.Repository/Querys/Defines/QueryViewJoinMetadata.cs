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
        }

        public Type JoinEntityType { get; set; }

        public Expression AliasExpression { get; set; }

        public Expression ConditionExpression { get; set; }

        public JoinType JoinType { get; set; }
    }
}
