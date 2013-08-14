using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CurdateFunc : FunctionExpression
    {
        public CurdateFunc()
            : base("CURDATE", null)
        {

        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CurdateFunc();
        }
    }
}
