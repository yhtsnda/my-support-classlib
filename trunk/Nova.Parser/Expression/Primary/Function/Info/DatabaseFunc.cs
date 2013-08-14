using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DatabaseFunc : FunctionExpression
    {
        public DatabaseFunc(List<IExpression> args)
            : base("DATABASE", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new DatabaseFunc(arguments);
        }
    }
}
