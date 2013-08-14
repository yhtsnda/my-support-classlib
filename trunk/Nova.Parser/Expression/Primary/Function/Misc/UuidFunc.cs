using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UuidFunc : FunctionExpression
    {
        public UuidFunc(List<IExpression> args)
            : base("UUID", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UuidFunc(arguments);
        }
    }
}
