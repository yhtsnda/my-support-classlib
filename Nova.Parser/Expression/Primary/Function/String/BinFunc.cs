using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BinFunc : FunctionExpression
    {
        public BinFunc(List<IExpression> args)
            : base("BIN", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new BinFunc(arguments);
        }
    }
}
