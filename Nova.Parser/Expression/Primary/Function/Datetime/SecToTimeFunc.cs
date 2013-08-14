using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SecToTimeFunc : FunctionExpression
    {
        public SecToTimeFunc(List<IExpression> arguments)
            : base("SEC_TO_TIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SecToTimeFunc(arguments);
        }
    }
}
