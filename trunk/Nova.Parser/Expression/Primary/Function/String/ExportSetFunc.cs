using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ExportSetFunc : FunctionExpression
    {
        public ExportSetFunc(List<IExpression> args)
            : base("EXPORT_SET", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ExportSetFunc(arguments);
        }
    }
}
