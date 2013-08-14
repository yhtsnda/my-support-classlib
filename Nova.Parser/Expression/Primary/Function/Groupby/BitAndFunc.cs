using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BitAndFunc : FunctionExpression
    {
        public BitAndFunc(List<IExpression> arguments)
            : base("IF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new BitAndFunc(arguments);
        }
    }
}
