using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CurrentUserFunc : FunctionExpression
    {
        public CurrentUserFunc()
            : base("CURRENT_USER", null)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CurrentUserFunc();
        }
    }
}
