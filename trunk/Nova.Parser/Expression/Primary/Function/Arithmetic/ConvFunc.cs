using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConvFunc : FunctionExpression
    {
        public ConvFunc(List<IExpression> arguments)
            : base("CONV", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ConvFunc(arguments);
        }
    }
}
