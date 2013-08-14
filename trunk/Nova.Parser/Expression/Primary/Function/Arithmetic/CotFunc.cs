using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CotFunc : FunctionExpression
    {
        public CotFunc(List<IExpression> arguments)
            : base("COT", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CotFunc(arguments);
        }
    }
}
