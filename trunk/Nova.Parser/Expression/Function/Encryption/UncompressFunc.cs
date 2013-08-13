using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UncompressFunc: FunctionExpression
    {
        public UncompressFunc(List<IExpression> arguments)
            : base("UNCOMPRESS", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UncompressFunc(arguments);
        }
    }
}
