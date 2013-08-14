using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DefaultValue : PrimaryExpression
    {
        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<DefaultValue>(this);
        }
    }
}
