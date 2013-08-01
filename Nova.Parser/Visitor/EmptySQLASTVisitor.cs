using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class EmptySQLASTVisitor : IASTVisitor
    {
        public void Visit<T>(T node) where T : IASTNode
        {
            throw new NotImplementedException();
        }
    }
}
