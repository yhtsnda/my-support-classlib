using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DateFormatFunc : FunctionExpression
    {
        public DateFormatFunc(List<IExpression> arguments)
            : base("DATE_FORMAT", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DateFormatFunc(arguments);
        }
    }
}
