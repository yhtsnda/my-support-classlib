using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class VersionFunc : FunctionExpression
    {
        public VersionFunc(List<IExpression> args)
            : base("VERSION", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new VersionFunc(arguments);
        }
    }
}
