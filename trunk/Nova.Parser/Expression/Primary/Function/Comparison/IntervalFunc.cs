using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IntervalFunc : FunctionExpression
    {
        public IntervalFunc(List<IExpression> arguments)
            : base("INTERVAL", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IntervalFunc(arguments);
        }
    }
}
