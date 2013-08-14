using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FoundRowsFunc : FunctionExpression
    {
        public FoundRowsFunc(List<IExpression> args)
            : base("FOUND_ROWS", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FoundRowsFunc(arguments);
        }
    }
}
