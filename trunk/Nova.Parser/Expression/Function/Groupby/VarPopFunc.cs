using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class VarPopFunc : FunctionExpression
    {
        public VarPopFunc(List<IExpression> arguments)
            : base("VAR_POP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new VarPopFunc(arguments);
        }
    }
}
