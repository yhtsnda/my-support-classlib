using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StrToDateFunc : FunctionExpression
    {
        public StrToDateFunc(List<IExpression> arguments)
            : base("STR_TO_DATE", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new StrToDateFunc(arguments);
        }
    }
}
