using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DayofMonthFunc : FunctionExpression
    {
        public DayofMonthFunc(List<IExpression> arguments)
            : base("DAYOFMONTH", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DayofMonthFunc(arguments);
        }
    }
}
