using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ReplaceStatement : InsertReplaceStatement
    {
        public enum ReplaceMode
        {
            Undefine,
            Low,
            Delay
        }
        public ReplaceStatement(ReplaceMode mode, bool ignore, Identifier table,
            IList<Identifier> columnList, IList<RowExpression> rowList)
            : base(table, columnList, rowList)
        {
            this.Mode = mode;
        }

        public ReplaceStatement(ReplaceMode mode, bool ignore, Identifier table, 
            IList<Identifier> columnList, IQueryExpression select)
            : base(table, columnList, select)
        {
            this.Mode = mode;
        }

        public ReplaceMode Mode { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ReplaceStatement>(this);
        }
    }
}
