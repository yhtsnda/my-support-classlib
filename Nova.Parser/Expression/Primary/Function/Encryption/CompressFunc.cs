using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CompressFunc: FunctionExpression
    {
        public CompressFunc(List<IExpression> arguments)
            : base("COMPRESS", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CompressFunc(arguments);
        }
    }
}
