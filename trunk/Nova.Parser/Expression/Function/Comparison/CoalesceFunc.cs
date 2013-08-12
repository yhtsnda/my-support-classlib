using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CoalesceFunc : FunctionExpression
    {
        public CoalesceFunc(List<IExpression> arguments)
            : base("COALESCE", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CoalesceFunc(arguments);
        }
    }
}
