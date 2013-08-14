using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LeftFunc : FunctionExpression
    {
        public LeftFunc(List<IExpression> args)
            : base("LEFT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LeftFunc(arguments);
        }
    }
}
