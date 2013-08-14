using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LtrimFunc : FunctionExpression
    {
        public LtrimFunc(List<IExpression> args)
            : base("LTRIM", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LtrimFunc(arguments);
        }
    }
}
