using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class IfFunc : FunctionExpression
    {
        public IfFunc(List<IExpression> arguments)
            : base("IF", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new IfFunc(arguments);
        }
    }
}
