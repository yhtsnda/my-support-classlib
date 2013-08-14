using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SqrtFunc : FunctionExpression
    {
        public SqrtFunc(List<IExpression> arguments)
            : base("SQRT", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SqrtFunc(arguments);
        }
    }
}
