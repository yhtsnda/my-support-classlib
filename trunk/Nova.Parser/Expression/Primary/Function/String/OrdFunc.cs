using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class OrdFunc : FunctionExpression
    {
        public OrdFunc(List<IExpression> args)
            : base("ORD", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new OrdFunc(arguments);
        }
    }
}
