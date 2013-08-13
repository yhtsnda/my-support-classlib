using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Md5Func: FunctionExpression
    {
        public Md5Func(List<IExpression> arguments)
            : base("MD5", arguments)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new Md5Func(arguments);
        }
    }
}
