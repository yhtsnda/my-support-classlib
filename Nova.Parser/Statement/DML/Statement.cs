using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class Statement : IStatement
    {
        public abstract void Accept(IASTVisitor visitor);
    }
}
