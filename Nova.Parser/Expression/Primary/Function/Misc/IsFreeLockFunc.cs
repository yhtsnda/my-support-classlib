using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IsFreeLockFunc : FunctionExpression
    {
        public IsFreeLockFunc(List<IExpression> args)
            : base("IS_FREE_LOCK", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IsFreeLockFunc(arguments);
        }
    }
}
