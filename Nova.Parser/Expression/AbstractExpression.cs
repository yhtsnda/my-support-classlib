using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class AbstractExpression : IExpression
    {
        private object unEvaluatable = new object();
        private bool cacheEvalRst = true;
        private bool evaluated;

        public object UnEvaluatable
        {
            get { return this.unEvaluatable; }
        }

        public IExpression SetCacheEvalRst(bool cacheEvalRst)
        {
            this.cacheEvalRst = cacheEvalRst;
            return this;
        }

        public abstract object Evaluation<K, V>(IDictionary<K, V> parameters);
        public abstract ExpressionPrecedence GetPrecedence();
        public abstract void Accept(IASTVisitor visitor);
        protected abstract Object EvaluationInternal(IDictionary<Object, Object> parameters);
    }
}
