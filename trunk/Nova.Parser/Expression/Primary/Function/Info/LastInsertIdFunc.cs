using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LastInsertIdFunc : FunctionExpression
    {
        public LastInsertIdFunc(List<IExpression> args)
            : base("LAST_INSERT_ID", args)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LastInsertIdFunc(arguments);
        }
    }
}
