using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class OldPasswordFunc: FunctionExpression
    {
        public OldPasswordFunc(List<IExpression> arguments)
            : base("OLD_PASSWORD", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new OldPasswordFunc(arguments);
        }
    }
}
