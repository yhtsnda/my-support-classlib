using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ReplaceFunc : FunctionExpression
    {
        public ReplaceFunc(List<IExpression> args)
            : base("REPLACE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ReplaceFunc(arguments);
        }
    }
}
