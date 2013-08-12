using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RandFunc : FunctionExpression
    {
        public RandFunc(List<IExpression> arguments)
            : base("RAND", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RandFunc(arguments);
        }
    }
}
