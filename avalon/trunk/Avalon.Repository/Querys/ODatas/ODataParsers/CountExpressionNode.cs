using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class CountExpressionNode : ExpressionNode
    {
        public bool Value
        {
            get { return (bool)((ConstantExpressionNode)Expressions.First()).Value; }
        }
    }
}
