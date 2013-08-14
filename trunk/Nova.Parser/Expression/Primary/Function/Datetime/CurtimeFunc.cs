using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CurtimeFunc : FunctionExpression
    {
        public CurtimeFunc(List<IExpression> arguments)
            : base("CURTIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CurtimeFunc(arguments);
        }
    }
}
