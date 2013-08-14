using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LogFunc : FunctionExpression
    {
        public LogFunc(List<IExpression> arguments)
            : base("LOG", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LogFunc(arguments);
        }
    }
}
