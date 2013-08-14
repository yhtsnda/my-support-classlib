using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FindInSetFunc : FunctionExpression
    {
        public FindInSetFunc(List<IExpression> args)
            : base("FIND_IN_SET", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FindInSetFunc(arguments);
        }
    }
}
