using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class AbstractExpression : IExpression
    {
        private object unEvaluatable = new object();

        public object UnEvaluatable
        {
            get { return this.unEvaluatable; }
        }

        public IExpression SetCacheEvalRst()
        {
            throw new NotImplementedException();
        }

        public object Evaluation<K, V>(IDictionary<K, V> parameters)
        {
            throw new NotImplementedException();
        }

        public abstract ExpressionPrecedence GetPrecedence();
        public abstract void Accept(IASTVisitor visitor);
        protected abstract Object EvaluationInternal<K, V>(IDictionary<K, V> parameters);
    }
}
