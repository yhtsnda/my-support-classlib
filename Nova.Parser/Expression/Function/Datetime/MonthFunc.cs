using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MonthFunc : FunctionExpression
    {
        public MonthFunc(List<IExpression> arguments)
            : base("MONTH", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MonthFunc(arguments);
        }
    }
}
