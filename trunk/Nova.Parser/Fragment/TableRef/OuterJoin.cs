using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class OuterJoin : ITableReference
    {
        public OuterJoin(bool isLeftJoin, ITableReference leftTableRef, ITableReference rightTableRef,
            IExpression onCondition, IList<string> usings)
        {
            this.IsLeftJoin = isLeftJoin;
            this.LeftTableRef = leftTableRef;
            this.RightTableRef = rightTableRef;
            this.OnCondition = onCondition;
            this.Usings = usings;
        }

        public OuterJoin(bool isLeftJoin, ITableReference leftTableRef, ITableReference rightTableRef, 
            IExpression onCondition)
            : this(isLeftJoin, leftTableRef, rightTableRef, onCondition, null)
        {
        }

        public OuterJoin(bool isLeftJoin, ITableReference leftTableRef, ITableReference rightTableRef, 
            IList<string> usings)
            : this(isLeftJoin, leftTableRef, rightTableRef, null, usings)
        {
        }

        public bool IsLeftJoin { get; protected set; }
        public ITableReference LeftTableRef { get; protected set; }
        public ITableReference RightTableRef { get; protected set; }
        public IExpression OnCondition { get; protected set; }
        public IList<string> Usings { get; protected set; }

        public bool IsSingleTable()
        {
            return false;
        }

        public object RemoveLastConditionElement()
        {
            return null;
        }

        public TableReferencePrecedence GetPrecedence()
        {
            return TableReferencePrecedence.Join;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit<OuterJoin>(this);
        }
    }
}
