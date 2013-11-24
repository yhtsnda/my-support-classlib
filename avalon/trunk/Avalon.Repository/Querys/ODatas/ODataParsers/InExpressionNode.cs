using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class InExpressionNode : ExpressionNode
    {
        public InExpressionNode(bool notIn)
        {
            Mode = notIn ? InMode.NotIn : InMode.In;
        }

        public InMode Mode { get; set; }

        public string PropertyName
        {
            get { return ((PropertyExpressionNode)Expressions[0]).PropertyName; }
        }

        public ConstantExpressionNode[] Constants
        {
            get { return Expressions.OfType<ConstantExpressionNode>().ToArray(); }
        }
    }

    public enum InMode
    {
        In,
        NotIn
    }
}
