using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class InnerJoin : ITableReference
    {
        public InnerJoin(ITableReference leftTableRef, ITableReference rightTableRef,
            IExpression onCondition, IList<string> usings)
            : base()
        {
            this.LeftTableRef = leftTableRef;
            this.RightTableRef = rightTableRef;
            this.OnCondition = onCondition;
            this.Usings = usings;
        }

        public InnerJoin(ITableReference leftTableRef, ITableReference rightTableRef)
            : this(leftTableRef, rightTableRef, null, null)
        {
        }

        public InnerJoin(ITableReference leftTableRef, ITableReference rightTableRef, IExpression onCondition)
            : this(leftTableRef, rightTableRef, onCondition, null)
        {
        }

        public InnerJoin(ITableReference leftTableRef, ITableReference rightTableRef, IList<string> usings)
            : this(leftTableRef, rightTableRef, null, usings)
        {
        }

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
            object tmpObj;

            if (OnCondition != null)
            {
                tmpObj = OnCondition;
                OnCondition = null;
            }
            else if (Usings != null)
            {
                tmpObj = Usings;
                Usings = null;
            }
            else
            {
                return null;
            }
            return tmpObj;
        }

        public TableReferencePrecedence GetPrecedence()
        {
            return TableReferencePrecedence.Join;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit<InnerJoin>(this);
        }
    }
}
