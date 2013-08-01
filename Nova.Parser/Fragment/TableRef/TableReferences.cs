using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TableReferences : ITableReference
    {
        private readonly IList<ITableReference> tableReferences;

        public TableReferences(IList<ITableReference> list)
        {
            this.tableReferences = list;
        }
    
        public bool IsSingleTable()
        {
            throw new NotImplementedException();
        }

        public void RemoveLastConditionElement()
        {
            throw new NotImplementedException();
        }

        public void GetPrecedence()
        {
            throw new NotImplementedException();
        }

        public void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
