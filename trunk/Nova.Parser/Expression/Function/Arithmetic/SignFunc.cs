using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SignFunc : FunctionExpression
    {
        public SignFunc(List<IExpression> arguments)
            : base("SIGN", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SignFunc(arguments);
        }
    }
}
