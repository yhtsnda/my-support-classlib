using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DayofYearFunc : FunctionExpression
    {
        public DayofYearFunc(List<IExpression> arguments)
            : base("DAYOFYEAR", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DayofYearFunc(arguments);
        }
    }
}
