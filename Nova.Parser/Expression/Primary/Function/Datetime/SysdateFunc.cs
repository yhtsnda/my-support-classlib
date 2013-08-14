using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SysdateFunc : FunctionExpression
    {
        public SysdateFunc(List<IExpression> arguments)
            : base("SYSDATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SysdateFunc(arguments);
        }
    }
}
