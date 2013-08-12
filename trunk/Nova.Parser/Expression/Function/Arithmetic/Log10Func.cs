using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Log10Func : FunctionExpression
    {
        public Log10Func(List<IExpression> arguments)
            : base("LOG10", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Log10Func(arguments);
        }
    }
}
