using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DaynameFunc : FunctionExpression
    {
        public DaynameFunc(List<IExpression> arguments)
            : base("DAYNAME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DaynameFunc(arguments);
        }
    }
}
