using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StddevSampFunc : FunctionExpression
    {
        public StddevSampFunc(List<IExpression> arguments)
            : base("STDDEV_SAMP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StddevSampFunc(arguments);
        }
    }
}
