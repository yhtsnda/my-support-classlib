using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DateAddFunc : FunctionExpression
    {
        public DateAddFunc(List<IExpression> arguments)
            : base("DATE_ADD", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DateAddFunc(arguments);
        }
    }
}
