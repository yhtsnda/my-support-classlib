using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CeilingFunc : FunctionExpression
    {
        public CeilingFunc(List<IExpression> arguments)
            : base("CEILING", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CeilingFunc(arguments);
        }
    }
}
