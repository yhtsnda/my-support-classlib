using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LengthFunc : FunctionExpression
    {
        public LengthFunc(List<IExpression> args)
            : base("LENGTH", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LengthFunc(arguments);
        }
    }
}
