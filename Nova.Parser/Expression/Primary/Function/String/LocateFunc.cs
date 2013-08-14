using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LocateFunc : FunctionExpression
    {
        public LocateFunc(List<IExpression> args)
            : base("LOCATE", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new LocateFunc(arguments);
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LocateFunc>(this);
        }
    }
}
