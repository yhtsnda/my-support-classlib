using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public abstract class ValueExpressionNode : ExpressionNode
    {
        public int Value
        {
            get
            {
                return (int)((ConstantExpressionNode)Expressions.First()).Value;
            }
        }
    }
}
