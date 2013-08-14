using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InetNtoaFunc : FunctionExpression
    {
        public InetNtoaFunc(List<IExpression> args)
            : base("INET_NTOA", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new InetNtoaFunc(arguments);
        }
    }
}
