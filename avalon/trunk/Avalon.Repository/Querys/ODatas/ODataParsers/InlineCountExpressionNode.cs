using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class InlineCountExpressionNode : ExpressionNode
    {
        public string InlinCountType
        {
            get { return Expressions.OfType<InlineCountTypeExpressionNode>().First().InlinCountType; }
        }
    }

    public class InlineCountTypeExpressionNode : ExpressionNode
    {
        public string InlinCountType { get; set; }
    }
}
