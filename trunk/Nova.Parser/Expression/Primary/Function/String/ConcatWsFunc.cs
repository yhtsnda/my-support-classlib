using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConcatWsFunc : FunctionExpression
    {
        public ConcatWsFunc(List<IExpression> args)
            : base("CONCAT_WS", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ConcatWsFunc(arguments);
        }
    }
}
