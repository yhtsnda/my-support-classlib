using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ExistsPrimary : PrimaryExpression
    {
        public IQueryExpression SubQuery { get; protected set; }

        public ExistsPrimary(IQueryExpression subquery)
        {
            if (subquery == null)
                throw new ArgumentNullException("sqbquery is null for EXISTS expression");
            this.SubQuery = subquery;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ExistsPrimary>(this);
        }
    }
}
