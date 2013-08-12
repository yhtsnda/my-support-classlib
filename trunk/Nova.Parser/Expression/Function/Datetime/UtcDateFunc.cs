using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UtcDateFunc : FunctionExpression
    {
        public UtcDateFunc(List<IExpression> arguments)
            : base("UTC_DATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UtcDateFunc(arguments);
        }
    }
}
