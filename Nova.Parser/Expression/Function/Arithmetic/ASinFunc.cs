using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ASinFunc : FunctionExpression
    {
        public ASinFunc(List<IExpression> arguments)
            : base("ASIN", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ASinFunc(arguments);
        }
    }
}
