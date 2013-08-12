using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SecondFunc : FunctionExpression
    {
        public SecondFunc(List<IExpression> arguments)
            : base("SECOND", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SecondFunc(arguments);
        }
    }
}
