using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DegreesFunc : FunctionExpression
    {
        public DegreesFunc(List<IExpression> arguments)
            : base("DEGREES", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DegreesFunc(arguments);
        }
    }
}
