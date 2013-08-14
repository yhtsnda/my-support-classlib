using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IsUsedLockFunc : FunctionExpression
    {
        public IsUsedLockFunc(List<IExpression> args)
            : base("IS_USED_LOCK", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IsUsedLockFunc(arguments);
        }
    }
}
