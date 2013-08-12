using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class WeekdayFunc : FunctionExpression
    {
        public WeekdayFunc(List<IExpression> arguments)
            : base("WEEKDAY", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new WeekdayFunc(arguments);
        }
    }
}
