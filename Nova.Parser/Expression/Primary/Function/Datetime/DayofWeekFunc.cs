using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DayofWeekFunc : FunctionExpression
    {
        public DayofWeekFunc(List<IExpression> arguments)
            : base("DAYOFWEEK", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DayofWeekFunc(arguments);
        }
    }
}
