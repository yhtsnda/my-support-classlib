using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UpperFunc : FunctionExpression
    {
        public UpperFunc(List<IExpression> args)
            : base("UPPER", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UpperFunc(arguments);
        }
    }
}
