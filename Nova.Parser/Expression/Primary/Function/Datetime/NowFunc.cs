using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NowFunc : FunctionExpression
    {
        public NowFunc()
            : base("NOW", null)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new NowFunc();
        }
    }
}
