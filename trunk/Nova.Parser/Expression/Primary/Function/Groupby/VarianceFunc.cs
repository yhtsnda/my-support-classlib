using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class VarianceFunc : FunctionExpression
    {
        public VarianceFunc(List<IExpression> arguments)
            : base("VARIANCE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new VarianceFunc(arguments);
        }
    }
}
