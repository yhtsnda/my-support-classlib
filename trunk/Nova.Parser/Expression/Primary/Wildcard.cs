using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Wildcard : Identifier
    {
        public Wildcard(Identifier parent)
            : base(parent, "*", "*")
        {
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<Wildcard>(this);
        }
    }
}
