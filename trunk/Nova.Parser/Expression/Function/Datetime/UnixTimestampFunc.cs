using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UnixTimestampFunc : FunctionExpression
    {
        public UnixTimestampFunc(List<IExpression> arguments)
            : base("UNIX_TIMESTAMP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UnixTimestampFunc(arguments);
        }
    }
}
