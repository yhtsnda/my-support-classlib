using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class EltFunc : FunctionExpression
    {
        public EltFunc(List<IExpression> args)
            : base("ELT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new EltFunc(arguments);
        }
    }
}
