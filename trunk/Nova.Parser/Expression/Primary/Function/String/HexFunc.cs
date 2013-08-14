using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class HexFunc : FunctionExpression
    {
        public HexFunc(List<IExpression> args)
            : base("HEX", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new HexFunc(arguments);
        }
    }
}
