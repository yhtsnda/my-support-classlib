using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StdFunc : FunctionExpression
    {
        public StdFunc(List<IExpression> arguments)
            : base("STD", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StdFunc(arguments);
        }
    }
}
