using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StrcmpFunc : FunctionExpression
    {
        public StrcmpFunc(List<IExpression> args)
            : base("STRCMP", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StrcmpFunc(arguments);
        }
    }
}
