using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class WeekFunc : FunctionExpression
    {
        public WeekFunc(List<IExpression> arguments)
            : base("WEEK", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new WeekFunc(arguments);
        }
    }
}
