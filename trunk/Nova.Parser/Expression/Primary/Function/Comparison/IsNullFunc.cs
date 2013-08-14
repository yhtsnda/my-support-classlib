using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IsNullFunc : FunctionExpression
    {
        public IsNullFunc(List<IExpression> arguments)
            : base("ISNULL", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IsNullFunc(arguments);
        }
    }
}
