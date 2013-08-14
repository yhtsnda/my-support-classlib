using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CollationFunc : FunctionExpression
    {
        public CollationFunc(List<IExpression> args)
            : base("COLLATION", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CollationFunc(arguments);
        }
    }
}
