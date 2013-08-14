using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DefaultFunc : FunctionExpression
    {
        public DefaultFunc(List<IExpression> args)
            : base("DEFAULT", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DefaultFunc(arguments);
        }
    }
}
