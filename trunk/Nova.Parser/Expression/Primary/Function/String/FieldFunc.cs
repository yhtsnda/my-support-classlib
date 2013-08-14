using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class FieldFunc : FunctionExpression
    {
        public FieldFunc(List<IExpression> args)
            : base("FIELD", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new FieldFunc(arguments);
        }
    }
}
