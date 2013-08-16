using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Dual : ITableReference
    {
        public bool IsSingleTable()
        {
            return true;
        }

        public object RemoveLastConditionElement()
        {
            return null;
        }

        public TableReferencePrecedence GetPrecedence()
        {
            return TableReferencePrecedence.Factor;
        }

        public void Accept(IASTVisitor visitor)
        {
            visitor.Visit<Dual>(this);
        }
    }
}
