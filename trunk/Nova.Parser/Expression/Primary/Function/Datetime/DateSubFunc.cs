using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DateSubFunc : FunctionExpression
    {
        public DateSubFunc(List<IExpression> arguments)
            : base("DATE_SUB", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DateSubFunc(arguments);
        }
    }
}
