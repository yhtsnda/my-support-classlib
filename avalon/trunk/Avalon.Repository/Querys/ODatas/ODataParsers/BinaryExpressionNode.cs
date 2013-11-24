using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class BinaryExpressionNode : ExpressionNode
    {
        public ExpressionNode Left
        {
            get { return Expressions[0]; }
        }

        public ExpressionNode Right
        {
            get { return Expressions[1]; }
        }
    }
}
