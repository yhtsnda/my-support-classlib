using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Crc32Func : FunctionExpression
    {
        public Crc32Func(List<IExpression> arguments)
            : base("CRC32", arguments)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Crc32Func(arguments);
        }
    }
}
