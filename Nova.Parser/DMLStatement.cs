using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class DMLStatement : IStatement
    {
        public void Accept(IASTVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
