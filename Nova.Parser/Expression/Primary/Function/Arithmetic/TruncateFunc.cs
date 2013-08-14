using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TruncateFunc : FunctionExpression
    {
        public TruncateFunc(List<IExpression> arguments)
            : base("TRUNCATE", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new TruncateFunc(arguments);
        }
    }
}
