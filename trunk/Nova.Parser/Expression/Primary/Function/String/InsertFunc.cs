using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InsertFunc : FunctionExpression
    {
        public InsertFunc(List<IExpression> args)
            : base("INSERT", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new InsertFunc(arguments);
        }
    }
}
