using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class EncodeFunc: FunctionExpression
    {
        public EncodeFunc(List<IExpression> arguments)
            : base("ENCODE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new EncodeFunc(arguments);
        }
    }
}
