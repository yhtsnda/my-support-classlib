using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class OrderbyExpressionNode : ExpressionNode
    {
        public IList<OrderbyItemExpresionNode> Items
        {
            get
            {
                return Expressions.OfType<OrderbyItemExpresionNode>().ToList();
            }
        }
    }
}
