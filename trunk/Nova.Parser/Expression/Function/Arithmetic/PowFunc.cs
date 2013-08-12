using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PowFunc : FunctionExpression
    {
        public PowFunc(List<IExpression> arguments)
            : base("POW", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new PowFunc(arguments);
        }
    }
}
