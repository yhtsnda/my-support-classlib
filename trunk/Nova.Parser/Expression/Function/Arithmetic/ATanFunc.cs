using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ATanFunc : FunctionExpression
    {
        public ATanFunc(List<IExpression> arguments)
            : base("ATAN", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ATanFunc(arguments);
        }
    }
}
