using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ExtractValueFunc : FunctionExpression
    {
        public ExtractValueFunc(List<IExpression> args)
            : base("EXTRACTVALUE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ExtractValueFunc(arguments);
        }
    }
}
