using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public enum ExpressionNodeType
    {
        Unkonwn,

        Filter,
        Skip,
        Top,
        Select,
        Orderby,
        OrderbyItem,
        InlineCount,

        Not,
        And,
        Or,
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual,
        Constant,
        Property,
        Function,
        In,

        Null
    }
}
