using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ReleaseLockFunc : FunctionExpression
    {
        public ReleaseLockFunc(List<IExpression> args)
            : base("RELEASE_LOCK", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ReleaseLockFunc(arguments);
        }
    }
}
