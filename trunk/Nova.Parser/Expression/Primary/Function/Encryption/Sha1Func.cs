using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Sha1Func: FunctionExpression
    {
        public Sha1Func(List<IExpression> arguments)
            : base("SHA1", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Sha1Func(arguments);
        }
    }
}
