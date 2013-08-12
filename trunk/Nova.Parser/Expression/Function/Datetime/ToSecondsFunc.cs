using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ToSecondsFunc : FunctionExpression
    {
        public ToSecondsFunc(List<IExpression> arguments)
            : base("TO_SECONDS", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ToSecondsFunc(arguments);
        }
    }
}
