using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DateFunc : FunctionExpression
    {
        public DateFunc(List<IExpression> arguments)
            : base("DATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DateFunc(arguments);
        }
    }
}
