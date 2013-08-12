using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimestampFunc : FunctionExpression
    {
        public TimestampFunc(List<IExpression> arguments)
            : base("TIMESTAMP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TimestampFunc(arguments);
        }
    }
}
