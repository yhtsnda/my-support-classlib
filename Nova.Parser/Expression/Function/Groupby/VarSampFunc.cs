using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class VarSampFunc : FunctionExpression
    {
        public VarSampFunc(List<IExpression> arguments)
            : base("VAR_SAMP", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new VarSampFunc(arguments);
        }
    }
}
