using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ReverseFunc : FunctionExpression
    {
        public ReverseFunc(List<IExpression> args)
            : base("REVERSE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ReverseFunc(arguments);
        }
    }
}
