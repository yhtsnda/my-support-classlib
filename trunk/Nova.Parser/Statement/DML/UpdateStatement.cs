using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class UpdateStatement : Statement
    {
        public UpdateStatement(bool lowPriority, bool quick, bool ignore, 
            IDictionary<Identifier, IExpression> values,
            TableReferences tableRefs, IExpression where, OrderByFragment orderBy, LimitFragment limit)
        {
            this.LowPriority = lowPriority;
            this.Quick = quick;
            this.Ignore = ignore;
            if (values == null || values.Count() == 0)
                throw new ArgumentException("tableNames Can't be NULL");
            this.Values = values;
            if (tableRefs == null)
                throw new ArgumentException("tableRefs Can't be NULL");
            this.TableRefs = tableRefs;
            this.Where = where;
            this.OrderBy = orderBy;
            this.Limit = limit;
        }

        public bool LowPriority { get; protected set; }
        public bool Quick { get; protected set; }
        public bool Ignore { get; protected set; }
        public IDictionary<Identifier, IExpression> Values { get; protected set; }
        public TableReferences TableRefs { get; protected set; }
        public IExpression Where { get; protected set; }
        public OrderByFragment OrderBy { get; protected set; }
        public LimitFragment Limit { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<UpdateStatement>(this);
        }
    }
}
