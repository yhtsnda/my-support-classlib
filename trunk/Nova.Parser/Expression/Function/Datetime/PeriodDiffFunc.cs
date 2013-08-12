using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class PeriodDiffFunc : FunctionExpression
    {
        public PeriodDiffFunc(List<IExpression> arguments)
            : base("PERIOD_DIFF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new PeriodDiffFunc(arguments);
        }
    }
}
