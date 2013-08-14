using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MonthnameFunc : FunctionExpression
    {
        public MonthnameFunc(List<IExpression> arguments)
            : base("MONTHNAME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MonthnameFunc(arguments);
        }
    }
}
