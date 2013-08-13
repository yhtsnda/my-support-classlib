using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StddevFunc : FunctionExpression
    {
        public StddevFunc(List<IExpression> arguments)
            : base("STDDEV", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StddevFunc(arguments);
        }
    }
}
