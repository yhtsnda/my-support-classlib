using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FromDaysFunc : FunctionExpression
    {
        public FromDaysFunc(List<IExpression> arguments)
            : base("FROM_DAYS", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FromDaysFunc(arguments);
        }
    }
}
