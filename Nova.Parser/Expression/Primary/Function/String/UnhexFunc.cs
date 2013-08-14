using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UnhexFunc : FunctionExpression
    {
        public UnhexFunc(List<IExpression> args)
            : base("UNHEX", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UnhexFunc(arguments);
        }
    }
}
