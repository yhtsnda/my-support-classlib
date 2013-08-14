using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InetAtonFunc : FunctionExpression
    {
        public InetAtonFunc(List<IExpression> args)
            : base("INET_ATON", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new InetAtonFunc(arguments);
        }
    }
}
