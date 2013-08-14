using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LastDayFunc : FunctionExpression
    {
        public LastDayFunc(List<IExpression> arguments)
            : base("LAST_DAY", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LastDayFunc(arguments);
        }
    }
}
