using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class QuarterFunc : FunctionExpression
    {
        public QuarterFunc(List<IExpression> arguments)
            : base("QUARTER", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new QuarterFunc(arguments);
        }
    }
}
