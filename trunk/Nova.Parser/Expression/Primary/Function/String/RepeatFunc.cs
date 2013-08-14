using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RepeatFunc : FunctionExpression
    {
        public RepeatFunc(List<IExpression> args)
            : base("REPEAT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RepeatFunc(arguments);
        }
    }
}
