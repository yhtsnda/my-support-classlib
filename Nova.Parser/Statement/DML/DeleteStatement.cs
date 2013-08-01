using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class DeleteStatement : Statement
    {


        public DeleteStatement(bool lowPriority, bool quick, bool ignore,
            Identifier tableName)
            : this(lowPriority, quick, ignore, tableName, null)
        {
        }

        public DeleteStatement(bool lowPriority, bool quick, bool ignore,
            Identifier tableName, IExpression where)
            : this(lowPriority, quick, ignore, tableName, where, null, null)
        {
        }

        public DeleteStatement(bool lowPriority, bool quick, bool ignore,
            Identifier tableName, IExpression where, OrderByFragment orderBy, LimitFragment limit)
        {
            this.LowPriority = lowPriority;
            this.Quick = quick;
            this.Ignore = ignore;
            this.TableNames = new List<Identifier>(1) { tableName };
            this.TableRefs = null;
            this.Where = where;
            this.OrderBy = orderBy;
            this.Limit = limit;
        }

        public DeleteStatement(bool lowPriority, bool quick, bool ignore,
            IList<Identifier> tableNames, TableReferences tableRefs)
            : this(lowPriority, quick, ignore, tableNames, tableRefs, null)
        {

        }

        public DeleteStatement(bool lowPriority, bool quick, bool ignore,
            IList<Identifier> tableNames, TableReferences tableRefs, IExpression where)
        {
            this.LowPriority = lowPriority;
            this.Quick = quick;
            this.Ignore = ignore;
            if (tableNames == null || tableNames.Count() == 0)
                throw new ArgumentException("tableNames Can't be NULL");
            this.TableNames = tableNames;
            if (tableRefs == null)
                throw new ArgumentException("tableRefs Can't be NULL");
            this.TableRefs = tableRefs;
            this.Where = where;
            this.OrderBy = null;
            this.Limit = null;
        }

        public bool LowPriority { get; protected set; }
        public bool Quick { get; protected set; }
        public bool Ignore { get; protected set; }
        public IList<Identifier> TableNames { get; protected set; }
        public TableReferences TableRefs { get; protected set; }
        public IExpression Where { get; protected set; }
        public OrderByFragment OrderBy { get; protected set; }
        public LimitFragment Limit { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<DeleteStatement>(this);
        }
        
    }
}
