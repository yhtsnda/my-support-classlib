using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BitCountFunc : FunctionExpression
    {
        public BitCountFunc(List<IExpression> arguments)
            : base("BIT_COUNT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return BitCountFunc(arguments);
        }
    }
}
