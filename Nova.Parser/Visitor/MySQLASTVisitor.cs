using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MySQLASTVisitor : IASTVisitor
    {
        private IList<Object> EmptyObjList = new List<Object>();
        private IList<int> EmptyIntList = new List<int>();
        private StringBuilder appendable;
        private Object[] args;
        private int[] argsIndex;
        private IDictionary<PlaceHolder, Object> placeHolderToString;

        public MySQLASTVisitor(StringBuilder builder)
            : this(builder, null)
        {
        }

        public MySQLASTVisitor(StringBuilder builder, Object[] args)
        {
            appendable = builder == null ? new StringBuilder() : builder;
            this.args = args == null ? EmptyObjList.ToArray() : args;
            this.argsIndex = args == null ? 
                EmptyIntList.ToArray() : 
                new List<int>(args.Length).ToArray();
        }

        public void Visit<T>(T node) where T : IASTNode
        {
            throw new NotImplementedException();
        }
    }
}
