using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RpadFunc : FunctionExpression
    {
        public RpadFunc(List<IExpression> args)
            : base("RPAD", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RpadFunc(arguments);
        }
    }
}
