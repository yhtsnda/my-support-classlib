using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimeFormatFunc : FunctionExpression
    {
        public TimeFormatFunc(List<IExpression> arguments)
            : base("TIME_FORMAT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TimeFormatFunc(arguments);
        }
    }
}
