using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    /// <summary>
    /// 表达式的优先级定义
    /// </summary>
    public enum ExpressionPrecedence
    {
        Query = 1,
        Assignment = 2,
        LogicalOR = 3,
        LogicalXOR = 4,
        LogicalAnd = 5,
        LogicalNot = 6,
        BetweenAnd = 7,
        Comparision = 8,
        AnyAllSubQuery = 9,
        BitOR = 10,
        BitAnd = 11,
        BitShift = 12,
        ArithmeticTermOp = 13,
        ArithmeticFactorOp = 14,
        BitXOR = 15,
        UnaryOp = 16,
        Binary = 17,
        Collate = 18,
        Primary = 19,
    }
}
