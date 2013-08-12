using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimeToSecFunc : FunctionExpression
    {
        public TimeToSecFunc(List<IExpression> arguments)
            : base("TIME_TO_SEC", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TimeToSecFunc(arguments);
        }
    }
}
