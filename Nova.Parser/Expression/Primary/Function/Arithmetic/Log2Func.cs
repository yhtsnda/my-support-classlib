using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Log2Func : FunctionExpression
    {
        public Log2Func(List<IExpression> arguments)
            : base("LOG2", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Log2Func(arguments);
        }
    }
}
