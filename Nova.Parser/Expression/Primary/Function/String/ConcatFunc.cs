using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConcatFunc : FunctionExpression
    {
        public ConcatFunc(List<IExpression> args)
            : base("CONCAT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ConcatFunc(arguments);
        }
    }
}
