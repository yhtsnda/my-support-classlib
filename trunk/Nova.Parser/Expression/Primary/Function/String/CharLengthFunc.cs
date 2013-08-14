using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CharLengthFunc : FunctionExpression
    {
        public CharLengthFunc(List<IExpression> args)
            : base("CHAR_LENGTH", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CharLengthFunc(arguments);
        }
    }
}
