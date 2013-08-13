using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UncompressedLengthFunc: FunctionExpression
    {
        public UncompressedLengthFunc(List<IExpression> arguments)
            : base("UNCOMPRESSED_LENGTH", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UncompressedLengthFunc(arguments);
        }
    }
}
