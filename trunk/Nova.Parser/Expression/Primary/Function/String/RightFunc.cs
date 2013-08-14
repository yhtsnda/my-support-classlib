using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RightFunc : FunctionExpression
    {
        public RightFunc(List<IExpression> args)
            : base("RIGHT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RightFunc(arguments);
        }
    }
}
