using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IfNullFunc : FunctionExpression
    {
        public IfNullFunc(List<IExpression> arguments)
            : base("IFNULL", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IfNullFunc(arguments);
        }
    }
}
