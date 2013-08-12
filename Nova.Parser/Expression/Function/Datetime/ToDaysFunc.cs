using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ToDaysFunc : FunctionExpression
    {
        public ToDaysFunc(List<IExpression> arguments)
            : base("TO_DAYS", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ToDaysFunc(arguments);
        }
    }
}
