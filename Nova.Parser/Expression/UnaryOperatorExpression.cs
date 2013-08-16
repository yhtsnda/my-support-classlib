using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class UnaryOperatorExpression : AbstractExpression
    {
        public UnaryOperatorExpression(IExpression operand, ExpressionPrecedence precedence)
        {
            if (operand == null)
                throw new ArgumentNullException("operand");
            this.Operand = operand;
            this.Precedence = precedence;
        }

        public IExpression Operand { get; protected set; }
        public ExpressionPrecedence Precedence { get; protected set; }

        public abstract string GetOperator();

        public override ExpressionPrecedence GetPrecedence()
        {
            return Precedence;
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return UnEvaluatable;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<UnaryOperatorExpression>(this);
        }
    }
}
