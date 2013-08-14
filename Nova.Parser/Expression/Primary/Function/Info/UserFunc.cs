using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UserFunc : FunctionExpression
    {
        public UserFunc(List<IExpression> args)
            : base("USER", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UserFunc(arguments);
        }
    }
}
