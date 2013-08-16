using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class BinaryOperatorExpression : AbstractExpression
    {
        public IExpression LeftOprand { get; protected set; }
        public IExpression RightOprand { get; protected set; }
        public ExpressionPrecedence Precedence { get; protected set; }
        public bool LeftCombine { get; protected set; }

        protected BinaryOperatorExpression(IExpression leftOprand, IExpression rightOprand,
            ExpressionPrecedence precedence)
            : this(leftOprand, rightOprand, precedence, true)
        {
        }

        protected BinaryOperatorExpression(IExpression leftOprand, IExpression rightOprand,
            ExpressionPrecedence precedence, bool leftCombine)
        {
            this.LeftOprand = leftOprand;
            this.RightOprand = rightOprand;
            this.Precedence = precedence;
            this.LeftCombine = leftCombine;
        }

        public abstract string GetOperator();

        public override ExpressionPrecedence GetPrecedence()
        {
            return Precedence;
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return UnEvaluatable;
        }
    }
}
