using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FloorFunc : FunctionExpression
    {
        public FloorFunc(List<IExpression> arguments)
            : base("FLOOR", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FloorFunc(arguments);
        }
    }
}
