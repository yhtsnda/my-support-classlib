using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class OctFunc : FunctionExpression
    {
        public OctFunc(List<IExpression> arguments)
            : base("OCT", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new OctFunc(arguments);
        }
    }
}
