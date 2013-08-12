using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class HourFunc : FunctionExpression
    {
        public HourFunc(List<IExpression> arguments)
            : base("HOUR", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new HourFunc(arguments);
        }
    }
}
