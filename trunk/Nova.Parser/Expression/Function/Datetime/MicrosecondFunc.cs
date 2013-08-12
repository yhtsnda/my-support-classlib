using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MicrosecondFunc : FunctionExpression
    {
        public MicrosecondFunc(List<IExpression> arguments)
            : base("MICROSECOND", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MicrosecondFunc(arguments);
        }
    }
}
