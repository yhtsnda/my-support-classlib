using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NowFunc : FunctionExpression
    {
        public NowFunc(List<IExpression> arguments)
            : base("NOW", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new NowFunc(arguments);
        }
    }
}
