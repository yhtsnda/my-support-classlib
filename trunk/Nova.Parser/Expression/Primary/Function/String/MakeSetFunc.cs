using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MakeSetFunc : FunctionExpression
    {
        public MakeSetFunc(List<IExpression> args)
            : base("MAKE_SET", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MakeSetFunc(arguments);
        }
    }
}
