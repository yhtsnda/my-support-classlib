using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StddevPopFunc : FunctionExpression
    {
        public StddevPopFunc(List<IExpression> arguments)
            : base("STDDEV_POP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StddevPopFunc(arguments);
        }
    }
}
