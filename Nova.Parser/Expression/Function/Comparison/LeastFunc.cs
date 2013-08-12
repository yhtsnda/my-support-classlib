using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LeastFunc : FunctionExpression
    {
        public LeastFunc(List<IExpression> arguments)
            : base("LEAST", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LeastFunc(arguments);
        }
    }
}
