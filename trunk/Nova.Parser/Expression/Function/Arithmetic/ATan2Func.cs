using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ATan2Func : FunctionExpression
    {
        public ATan2Func(List<IExpression> arguments)
            : base("ATAN2", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ATan2Func(arguments);
        }
    }
}
