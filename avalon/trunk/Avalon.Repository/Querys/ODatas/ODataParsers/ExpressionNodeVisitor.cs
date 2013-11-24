using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Avalon.Framework.Querys
{
    public abstract class ExpressionNodeVisitor
    {
        public Expression Visit(ExpressionNode exp)
        {
            return VisitExpression(exp);
        }

        Expression VisitExpression(ExpressionNode exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionNodeType.Not:
                    return VisitNot((NotExpressionNode)exp);
                case ExpressionNodeType.And:
                case ExpressionNodeType.Or:
                case ExpressionNodeType.Equal:
                case ExpressionNodeType.LessThan:
                case ExpressionNodeType.LessThanOrEqual:
                case ExpressionNodeType.GreaterThan:
                case ExpressionNodeType.GreaterThanOrEqual:
                case ExpressionNodeType.NotEqual:
                    return VisitBinary((BinaryExpressionNode)exp);
                case ExpressionNodeType.Constant:
                    return VisitConstant((ConstantExpressionNode)exp);
                case ExpressionNodeType.Property:
                    return VisitProperty((PropertyExpressionNode)exp);
                case ExpressionNodeType.Function:
                    return VisitFunction((FunctionExpressionNode)exp);
                case ExpressionNodeType.In:
                    return VisitIn((InExpressionNode)exp);
                case ExpressionNodeType.Filter:
                    return VisitFilter((FilterExpressionNode)exp);
                default:
                    throw new NotSupportedException();
            }
        }

        protected abstract Expression VisitFilter(FilterExpressionNode exp);

        protected abstract Expression VisitIn(InExpressionNode exp);

        protected abstract Expression VisitNot(NotExpressionNode exp);

        protected abstract Expression VisitBinary(BinaryExpressionNode exp);

        protected abstract Expression VisitConstant(ConstantExpressionNode exp);

        protected abstract Expression VisitProperty(PropertyExpressionNode exp);

        protected abstract Expression VisitFunction(FunctionExpressionNode exp);
    }
}
