using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class GreatestFunc : FunctionExpression
    {
        public GreatestFunc(List<IExpression> arguments)
            : base("CREATEST", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new GreatestFunc(arguments);
        }
    }
}
