using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UuidShortFunc : FunctionExpression
    {
        public UuidShortFunc(List<IExpression> args)
            : base("UUID_SHORT", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UuidShortFunc(arguments);
        }
    }
}
