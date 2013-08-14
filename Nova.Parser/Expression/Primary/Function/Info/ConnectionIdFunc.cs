using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConnectionIdFunc : FunctionExpression
    {
        public ConnectionIdFunc(List<IExpression> args)
            : base("CONNECTION_ID", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ConnectionIdFunc(arguments);
        }
    }
}
