using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class NaturalJoin : ITableReference
    {
        public NaturalJoin(bool isOuter, bool isLeft, ITableReference leftTableRef, ITableReference rightTableRef)
        {
            this.IsOuter = isOuter;
            this.IsLeft = isLeft;
            this.LeftTableRef = leftTableRef;
            this.RightTableRef = rightTableRef;
        }

        public bool IsOuter { get; protected set; }
        public bool IsLeft { get; protected set; }
        public ITableReference LeftTableRef { get; protected set; }
        public ITableReference RightTableRef { get; protected set; }

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
            visitor.Visit<NaturalJoin>(this);
        }
    }
}
