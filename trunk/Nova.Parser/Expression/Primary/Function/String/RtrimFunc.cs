using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RtrimFunc : FunctionExpression
    {
        public RtrimFunc(List<IExpression> args)
            : base("RTRIM", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RtrimFunc(arguments);
        }
    }
}
