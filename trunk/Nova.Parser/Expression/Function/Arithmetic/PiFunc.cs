using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PiFunc : FunctionExpression
    {
        public PiFunc(List<IExpression> arguments)
            : base("PI", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new PiFunc(arguments);
        }
    }
}
