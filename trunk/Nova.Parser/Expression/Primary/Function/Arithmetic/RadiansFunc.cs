using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RadiansFunc : FunctionExpression
    {
        public RadiansFunc(List<IExpression> arguments)
            : base("RADIANS", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RadiansFunc(arguments);
        }
    }
}
