using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CoercibilityFunc : FunctionExpression
    {
        public CoercibilityFunc(List<IExpression> args)
            : base("COERCIBILITY", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CoercibilityFunc(arguments);
        }
    }
}
