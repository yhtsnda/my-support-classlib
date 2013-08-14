using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LpadFunc : FunctionExpression
    {
        public LpadFunc(List<IExpression> args)
            : base("LPAD", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LpadFunc(arguments);
        }
    }
}
