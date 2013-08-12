using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UtcTimestampFunc : FunctionExpression
    {
        public UtcTimestampFunc(List<IExpression> arguments)
            : base("UTC_TIMESTAMP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UtcTimestampFunc(arguments);
        }
    }
}
