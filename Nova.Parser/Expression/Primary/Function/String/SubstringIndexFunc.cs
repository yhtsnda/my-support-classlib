using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SubstringIndexFunc : FunctionExpression
    {
        public SubstringIndexFunc(List<IExpression> args)
            : base("SUBSTRING_INDEX", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SubstringIndexFunc(arguments);
        }
    }
}
