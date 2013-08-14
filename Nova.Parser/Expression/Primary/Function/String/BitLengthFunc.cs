using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BitLengthFunc : FunctionExpression
    {
        public BitLengthFunc(List<IExpression> args)
            : base("BIT_LENGTH", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new BitLengthFunc(arguments);
        }
    }
}
