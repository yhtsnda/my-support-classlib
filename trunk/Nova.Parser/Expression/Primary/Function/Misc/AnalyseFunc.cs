using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AnalyseFunc : FunctionExpression
    {
        public AnalyseFunc(List<IExpression> args)
            : base("ANALYSE", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new AnalyseFunc(arguments);
        }
    }
}
