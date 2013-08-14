using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AsciiFunc : FunctionExpression
    {
        public AsciiFunc(List<IExpression> args)
            : base("ASCII", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AsciiFunc(arguments);
        }
    }
}
