using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TanFunc : FunctionExpression
    {
        public TanFunc(List<IExpression> arguments)
            : base("TAN", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TanFunc(arguments);
        }
    }
}
