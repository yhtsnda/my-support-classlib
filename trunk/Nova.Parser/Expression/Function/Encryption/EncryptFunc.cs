using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class EncryptFunc: FunctionExpression
    {
        public EncryptFunc(List<IExpression> arguments)
            : base("ENCRYPT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new EncryptFunc(arguments);
        }
    }
}
