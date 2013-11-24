using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class OrderbyItemExpresionNode : ExpressionNode
    {
        public OrderbyItemExpresionNode(IToken token)
        {
            Type = token.Text;
        }

        public string Type { get; set; }

        public string PropertyName
        {
            get { return Expressions.OfType<PropertyExpressionNode>().First().PropertyName; }
        }
    }
}
