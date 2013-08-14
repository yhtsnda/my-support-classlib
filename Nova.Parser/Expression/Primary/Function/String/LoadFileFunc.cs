using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LoadFileFunc : FunctionExpression
    {
        public LoadFileFunc(List<IExpression> args)
            : base("LOAD_FILE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LoadFileFunc(arguments);
        }
    }
}
