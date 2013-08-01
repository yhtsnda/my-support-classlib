using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public interface IASTNode
    {
        void Accept(IASTVisitor visitor);
    }
}
