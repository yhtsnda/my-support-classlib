using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class YearFunc : FunctionExpression
    {
        public YearFunc(List<IExpression> arguments)
            : base("YEAR", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new YearFunc(arguments);
        }
    }
}
