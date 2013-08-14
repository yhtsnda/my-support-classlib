using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LowerFunc : FunctionExpression
    {
        public LowerFunc(List<IExpression> args)
            : base("LOWER", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LowerFunc(arguments);
        }
    }
}
