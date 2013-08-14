using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SubstringFunc : FunctionExpression
    {
        public SubstringFunc(List<IExpression> args)
            : base("SUBSTRING", args)
        {
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new SubstringFunc(arguments);
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<SubstringFunc>(this);
        }
    }
}
