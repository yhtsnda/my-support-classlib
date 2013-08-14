using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SoundexFund : FunctionExpression
    {
        public SoundexFund(List<IExpression> args)
            : base("SOUNDEX", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SoundexFund(arguments);
        }
    }
}
