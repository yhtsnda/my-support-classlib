using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MinuteFunc : FunctionExpression
    {
        public MinuteFunc(List<IExpression> arguments)
            : base("MINUTE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MinuteFunc(arguments);
        }
    }
}
