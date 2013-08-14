using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MaxFunc : FunctionExpression
    {
        public bool Distince { get; protected set; }

        public MaxFunc(IExpression expr, bool distince)
            : base("MAX", WrapList(expr))
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of max has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<MaxFunc>(this);
        }
    }
}
