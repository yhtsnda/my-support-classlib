using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class WeekofyearFunc : FunctionExpression
    {
        public WeekofyearFunc(List<IExpression> arguments)
            : base("WEEKOFYEAR", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new WeekofyearFunc(arguments);
        }
    }
}
