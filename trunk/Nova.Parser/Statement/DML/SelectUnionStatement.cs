using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SelectUnionStatement : QueryStatement
    {
        public SelectUnionStatement(SelectStatement select)
            : base()
        {
            this.SelectStatements = new ConcurrentQueue<SelectStatement>();
            this.SelectStatements.Enqueue(select);
        }

        public ConcurrentQueue<SelectStatement> SelectStatements { get; protected set; }
        public int FirstDistinctIndex { get; protected set; }
        public OrderByFragment Order { get; set; }
        public LimitFragment Limit { get; set; }

        public SelectUnionStatement AddSelect(SelectStatement select, bool unionAll)
        {
            this.SelectStatements.Enqueue(select);
            if (!unionAll)
                this.FirstDistinctIndex = SelectStatements.Count - 1;
            return this;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<SelectUnionStatement>(this);
        }
    }
}
