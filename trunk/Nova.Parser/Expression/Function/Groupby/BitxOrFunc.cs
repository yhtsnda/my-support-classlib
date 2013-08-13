using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BitxOrFunc : FunctionExpression
    {
        public BitxOrFunc(List<IExpression> arguments)
            : base("BIT_XOR", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new BitxOrFunc(arguments);
        }
    }
}
