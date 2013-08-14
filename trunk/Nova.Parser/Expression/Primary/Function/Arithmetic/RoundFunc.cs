using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RoundFunc : FunctionExpression
    {
        public RoundFunc(List<IExpression> arguments)
            : base("ROUND", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RoundFunc(arguments);
        }
    }
}
