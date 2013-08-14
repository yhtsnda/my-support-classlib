using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MinFunc : FunctionExpression
    {
        public bool Distince { get; protected set; }

        public MinFunc(IExpression expr, bool distince)
            : base("MIN", WrapList(expr))
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of min has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<MinFunc>(this);
        }
    }
}
