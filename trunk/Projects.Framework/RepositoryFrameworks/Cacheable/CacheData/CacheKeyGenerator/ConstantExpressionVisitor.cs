using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Projects.Framework
{
    internal class ConstantExpressionVisitor : ExpressionVisitor
    {
        bool isLambda = false;
        List<object> constants = new List<object>();

        public ConstantExpressionVisitor(Expression expression)
        {
            this.Visit(expression);
        }

        public List<object> Constants
        {
            get { return constants; }
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            isLambda = true;
            var expr = base.VisitLambda<T>(node);
            isLambda = false;
            return expr;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (isLambda)
                constants.Add(node.Value);
            return base.VisitConstant(node);
        }
    }
}
