using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AesDecryptFunc : FunctionExpression
    {
        public AesDecryptFunc(List<IExpression> arguments)
            : base("AES_DECRYPT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AesDecryptFunc(arguments);
        }
    }
}
