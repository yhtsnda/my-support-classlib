using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Sha2Func: FunctionExpression
    {
        public Sha2Func(List<IExpression> arguments)
            : base("SHA2", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Sha2Func(arguments);
        }
    }
}
