using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    [DebuggerTypeProxy(typeof(ExpressionDebugView))]
    public abstract class ExpressionNode : CommonTree
    {
        internal QueryTreeAdaptor TreeAdaptor { get; set; }

        public ExpressionNode ParentExpression
        {
            get
            {
                return base.Parent as ExpressionNode;
            }
        }

        public IList<ExpressionNode> Expressions
        {
            get
            {
                var nodes = new List<ExpressionNode>();
                if (base.ChildCount == 0)
                    return nodes;

                foreach (var child in base.Children)
                {
                    var node = child as ExpressionNode;
                    if (node != null)
                        nodes.Add(node);
                }
                return nodes;
            }
        }

        public ExpressionNodeType NodeType { get; set; }

        public IList<ExpressionNode> DescendantExpressions()
        {
            var items = new List<ExpressionNode>();
            DescendantExpressions(items);
            return items;
        }

        public IList<ExpressionNode> ParentExpressions()
        {
            var items = new List<ExpressionNode>();
            var parent = ParentExpression;
            while (parent != null)
            {
                items.Add(parent);
                parent = parent.ParentExpression;
            }
            return items;
        }

        void DescendantExpressions(List<ExpressionNode> items)
        {
            foreach (var exp in Expressions)
            {
                items.Add(exp);
                exp.DescendantExpressions(items);
            }
        }


        public override string ToString()
        {
            return NodeType.ToString() + ":" + base.ToString();
        }
    }
}
