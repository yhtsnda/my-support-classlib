using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NameConstFunc : FunctionExpression
    {
        public NameConstFunc(List<IExpression> args)
            : base("NAME_CONST", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new NameConstFunc(arguments);
        }
    }
}
