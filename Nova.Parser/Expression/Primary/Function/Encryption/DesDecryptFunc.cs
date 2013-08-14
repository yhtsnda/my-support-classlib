using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DesDecryptFunc: FunctionExpression
    {
        public DesDecryptFunc(List<IExpression> arguments)
            : base("DES_DECRYPT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DesDecryptFunc(arguments);
        }
    }
}
