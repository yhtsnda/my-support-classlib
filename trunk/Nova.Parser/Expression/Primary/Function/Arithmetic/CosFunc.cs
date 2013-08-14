using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CosFunc : FunctionExpression
    {
        public CosFunc(List<IExpression> arguments)
            : base("COS", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CosFunc(arguments);
        }
    }
}
