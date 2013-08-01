using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class RowExpression : PrimaryExpression
    {
        public RowExpression(IList<IExpression> rowExpressions)
        {
            if (rowExpressions == null || rowExpressions.Count == 0)
                RowExpressions = new List<IExpression>();
            else
                RowExpressions = rowExpressions;
        }

        public IList<IExpression> RowExpressions { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<RowExpression>(this);
        }
    }
}
