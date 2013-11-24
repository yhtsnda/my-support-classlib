using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    internal class ExpressionDebugView
    {
        private readonly ExpressionNode exp;

        public ExpressionDebugView(ExpressionNode exp)
        {
            this.exp = exp;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public ExpressionNode[] Expressions
        {
            get
            {
                if ((this.exp == null) || (this.exp.Expressions == null))
                    return null;

                ExpressionNode[] children = new ExpressionNode[this.exp.Expressions.Count];
                exp.Expressions.CopyTo(children, 0);
                return children;
            }

        }
    }
}
