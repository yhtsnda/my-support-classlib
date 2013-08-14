using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class YearweekFunc : FunctionExpression
    {
        public YearweekFunc(List<IExpression> arguments)
            : base("YEARWEEK", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new YearweekFunc(arguments);
        }
    }
}
