using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DecodeFunc: FunctionExpression
    {
        public DecodeFunc(List<IExpression> arguments)
            : base("DECODE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DecodeFunc(arguments);
        }
    }
}
