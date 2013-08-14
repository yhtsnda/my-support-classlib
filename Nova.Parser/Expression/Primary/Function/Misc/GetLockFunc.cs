using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class GetLockFunc : FunctionExpression
    {
        public GetLockFunc(List<IExpression> args)
            : base("GET_LOCK", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new GetLockFunc(arguments);
        }
    }
}
