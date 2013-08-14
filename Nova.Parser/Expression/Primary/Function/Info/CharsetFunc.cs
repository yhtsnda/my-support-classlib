using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CharsetFunc : FunctionExpression
    {
        public CharsetFunc(List<IExpression> args)
            : base("CHARSET", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CharsetFunc(arguments);
        }
    }
}
