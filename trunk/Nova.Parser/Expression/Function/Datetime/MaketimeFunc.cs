using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MaketimeFunc : FunctionExpression
    {
        public MaketimeFunc(List<IExpression> arguments)
            : base("MAKETIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MaketimeFunc(arguments);
        }
    }
}
