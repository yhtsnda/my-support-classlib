using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SleepFunc : FunctionExpression
    {
        public SleepFunc(List<IExpression> args)
            : base("SLEEP", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SleepFunc(arguments);
        }
    }
}
