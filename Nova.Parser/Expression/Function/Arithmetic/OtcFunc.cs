using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class OtcFunc : FunctionExpression
    {
        public OtcFunc(List<IExpression> arguments)
            : base("OTC", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new OtcFunc(arguments);
        }
    }
}
