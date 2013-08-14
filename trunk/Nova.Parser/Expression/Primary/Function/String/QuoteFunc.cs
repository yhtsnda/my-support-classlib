using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class QuoteFunc : FunctionExpression
    {
        public QuoteFunc(List<IExpression> args)
            : base("QUOTE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new QuoteFunc(arguments);
        }
    }
}
