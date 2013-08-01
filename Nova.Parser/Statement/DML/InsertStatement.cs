using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InsertStatement : InsertReplaceStatement
    {
        public enum InsertMode
        {
            Undefine,
            Low,
            Delay,
            High
        }

        public InsertStatement(InsertMode mode, bool ignore, Identifier table,
            IList<Identifier> columnList, IList<RowExpression> rowList,
            IDictionary<Identifier, IExpression> duplicateUpdate)
            : base(table, columnList, rowList)
        {
            this.Mode = mode;
            this.Ignore = ignore;
            this.DuplicateUpdate = duplicateUpdate;
        }

        public InsertStatement(InsertMode mode, bool ignore, Identifier table, 
            IList<Identifier> columnList, IQueryExpression select, 
            IDictionary<Identifier, IExpression> duplicateUpdate)
            : base(table, columnList, select)
        {
            this.Mode = mode;
            this.Ignore = ignore;
            this.DuplicateUpdate = duplicateUpdate;
        }

        public InsertMode Mode { get; protected set; }
        public IDictionary<Identifier, IExpression> DuplicateUpdate { get; protected set; }
        public bool Ignore { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<InsertStatement>(this);
        }
    }
}
