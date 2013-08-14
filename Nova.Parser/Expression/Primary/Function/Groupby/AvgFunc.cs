using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class AvgFunc : FunctionExpression
    {
        public bool Distince { get; protected set; }

        public AvgFunc(IExpression expr, bool distince)
            : base("AVG", WrapList(expr))
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of char has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<AvgFunc>(this);
        }
    }
}
