using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class TernaryOperatorExpression : AbstractExpression
    {
        public IExpression First { get; protected set; }
        public IExpression Second { get; protected set; }
        public IExpression Third { get; protected set; }

        public TernaryOperatorExpression(IExpression first, IExpression second, IExpression third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return UnEvaluatable;
        }
    }
}
