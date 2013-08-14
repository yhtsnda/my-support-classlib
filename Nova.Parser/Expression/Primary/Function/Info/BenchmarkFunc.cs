using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class BenchmarkFunc : FunctionExpression
    {
        public BenchmarkFunc(List<IExpression> args)
            : base("BENCHMARK", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new BenchmarkFunc(arguments);
        }
    }
}
