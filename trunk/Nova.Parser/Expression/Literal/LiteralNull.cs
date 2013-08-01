using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralNull : Literal
    {
        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return null;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralNull>(this);
        }
    }
}
