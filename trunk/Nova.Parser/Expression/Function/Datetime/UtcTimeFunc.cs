using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UtcTimeFunc : FunctionExpression
    {
        public UtcTimeFunc(List<IExpression> arguments)
            : base("UTC_TIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UtcTimeFunc(arguments);
        }
    }
}
