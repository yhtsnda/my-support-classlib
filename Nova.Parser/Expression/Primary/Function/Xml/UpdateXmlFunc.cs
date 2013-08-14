using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UpdateXmlFunc : FunctionExpression
    {
        public UpdateXmlFunc(List<IExpression> args)
            : base("UPDATEXML", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new UpdateXmlFunc(arguments);
        }
    }
}
