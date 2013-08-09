using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class PrimaryExpression : AbstractExpression
    {
        public override ExpressionPrecedence GetPrecedence()
        {
            return ExpressionPrecedence.Primary;
        }

        protected override object EvaluationInternal(IDictionary<Object, Object> parameters)
        {
            return UnEvaluatable;
        }

        public abstract void Accept(IASTVisitor visitor);

        public virtual object Evaluation<K, V>(IDictionary<K, V> parameters)
        {
            return null;
        }
    }
}
