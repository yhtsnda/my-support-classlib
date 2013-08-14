using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SubtimeFunc : FunctionExpression
    {
        public SubtimeFunc(List<IExpression> arguments)
            : base("SUBTIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SubtimeFunc(arguments);
        }
    }
}
