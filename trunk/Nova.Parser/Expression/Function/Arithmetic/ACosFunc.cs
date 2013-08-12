using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ACosFunc : FunctionExpression
    {
        public ACosFunc(List<IExpression> arguments)
            : base("ACOS", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ACosFunc(arguments);
        }
    }
}
