using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AesEncryptFunc: FunctionExpression
    {
        public AesEncryptFunc(List<IExpression> arguments)
            : base("AES_ENCRYPT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AesEncryptFunc(arguments);
        }
    }
}
