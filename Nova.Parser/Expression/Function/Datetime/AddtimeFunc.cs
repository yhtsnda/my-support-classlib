using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AddtimeFunc : FunctionExpression
    {
        public AddtimeFunc(List<IExpression> arguments)
            : base("ADDTIME", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AddtimeFunc(arguments);
        }
    }
}
