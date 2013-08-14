using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RowCountFunc : FunctionExpression
    {
        public RowCountFunc(List<IExpression> args)
            : base("ROW_COUNT", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new RowCountFunc(arguments);
        }
    }
}
