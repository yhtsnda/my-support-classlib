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

        protected override object EvaluationInternal<K, V>(IDictionary<K, V> parameters)
        {
            return UnEvaluatable;
        }
    }
}
