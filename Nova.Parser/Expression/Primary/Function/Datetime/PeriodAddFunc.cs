using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PeriodAddFunc : FunctionExpression
    {
        public PeriodAddFunc(List<IExpression> arguments)
            : base("PERIOD_ADD", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new PeriodAddFunc(arguments);
        }
    }
}
