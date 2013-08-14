using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SinFunc : FunctionExpression
    {
        public SinFunc(List<IExpression> arguments)
            : base("SIN", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SinFunc(arguments);
        }
    }
}
