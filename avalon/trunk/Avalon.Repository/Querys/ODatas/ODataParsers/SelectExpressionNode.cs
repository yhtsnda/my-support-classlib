using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class SelectExpressionNode : ExpressionNode
    {
        public string[] PropertyNames
        {
            get { return Expressions.OfType<PropertyExpressionNode>().Select(o => o.PropertyName).ToArray(); }
        }
    }
}
