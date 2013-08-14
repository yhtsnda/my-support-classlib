using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimeFunc : FunctionExpression
    {
        public TimeFunc(List<IExpression> arguments)
            : base("TIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TimeFunc(arguments);
        }
    }
}
