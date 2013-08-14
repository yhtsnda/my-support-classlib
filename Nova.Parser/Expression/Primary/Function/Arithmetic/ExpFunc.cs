using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ExpFunc : FunctionExpression
    {
        public ExpFunc(List<IExpression> arguments)
            : base("EXP", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ExpFunc(arguments);
        }
    }
}
