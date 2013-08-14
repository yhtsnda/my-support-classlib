using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InstrFunc : FunctionExpression
    {
        public InstrFunc(List<IExpression> args)
            : base("INSTR", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new InstrFunc(arguments);
        }
    }
}
