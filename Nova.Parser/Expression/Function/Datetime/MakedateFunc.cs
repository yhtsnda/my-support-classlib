using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MakedateFunc : FunctionExpression
    {
        public MakedateFunc(List<IExpression> arguments)
            : base("MAKEDATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new MakedateFunc(arguments);
        }
    }
}
