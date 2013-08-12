using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FromUnixtimeFunc : FunctionExpression
    {
        public FromUnixtimeFunc(List<IExpression> arguments)
            : base("FROM_UNIXTIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FromUnixtimeFunc(arguments);
        }
    }
}
