using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class FunctionExpressionNode : ExpressionNode
    {
        public FunctionExpressionNode(string functionName)
        {
            FunctionName = functionName; 
        }

        public string FunctionName { get; set; }

        public ExpressionNode[] Arguments
        {
            get { return Expressions.ToArray(); }
        }
    }
}
