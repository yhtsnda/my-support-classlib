using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AdddateFunc : FunctionExpression
    {
        public AdddateFunc(List<IExpression> arguments)
            : base("ADDDATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AdddateFunc(arguments);
        }
    }
}
