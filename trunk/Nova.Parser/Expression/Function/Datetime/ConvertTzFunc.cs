using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConvertTzFunc : FunctionExpression
    {
        public ConvertTzFunc(List<IExpression> arguments)
            : base("CONVERT_TZ", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ConvertTzFunc(arguments);
        }
    }
}
