using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FormatFunc : FunctionExpression
    {
        public FormatFunc(List<IExpression> args)
            : base("FORMAT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FormatFunc(arguments);
        }
    }
}
