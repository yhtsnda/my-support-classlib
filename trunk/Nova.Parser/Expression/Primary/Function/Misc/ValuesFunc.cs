using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ValuesFunc : FunctionExpression
    {
        public ValuesFunc(List<IExpression> args)
            : base("VALUES", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new ValuesFunc(arguments);
        }
    }
}
