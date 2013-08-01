using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class StraightJoin : ITableReference
    {
        public StraightJoin(ITableReference leftTableRef, ITableReference rightTableRef, IExpression onCondition)
            : base()
        {
            this.LeftTableRef = leftTableRef;
            this.RightTableRef = rightTableRef;
            this.OnCondition = onCondition;
        }

        public StraightJoin(ITableReference leftTableRef, ITableReference rightTableRef)
            : this(leftTableRef, rightTableRef, null)
        {
        }

        public ITableReference LeftTableRef { get; protected set; }
        public ITableReference RightTableRef { get; protected set; }
        public IExpression OnCondition { get; protected set; }

        public bool IsSingleTable()
        {
            throw new NotImplementedException();
        }

        public object RemoveLastConditionElement()
        {
            throw new NotImplementedException();
        }

        public TableReferencePrecedence GetPrecedence()
        {
            throw new NotImplementedException();
        }

        public void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
