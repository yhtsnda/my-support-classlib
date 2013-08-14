using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DateDiffFunc : FunctionExpression
    {
        public DateDiffFunc(List<IExpression> arguments)
            : base("DATEDIFF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DateDiffFunc(arguments);
        }
    }
}
