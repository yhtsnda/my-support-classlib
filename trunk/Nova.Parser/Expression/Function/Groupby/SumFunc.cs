using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SumFunc : FunctionExpression
    {
        public bool Distince { get; protected set; }

        public SumFunc(IExpression expr, bool distince)
            : base("SUM", WrapList(expr))
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of char has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<SumFunc>(this);
        }
    }
}
