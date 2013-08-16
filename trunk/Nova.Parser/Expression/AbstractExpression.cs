using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class AbstractExpression : IExpression
    {
        private object unevaluatable = new object();
        public object UnEvaluatable
        {
            get { return unevaluatable; }
        }

        private object evaluationCache = new object();
        private bool cacheEvalRst = true;
        private bool evaluated;

        public virtual IExpression SetCacheEvalRst(bool cacheEvalRst)
        {
            this.cacheEvalRst = cacheEvalRst;
            return this;
        }

        public virtual object Evaluation(IDictionary<object, object> parameters)
        {
            if (cacheEvalRst)
            {
                if (evaluated)
                    return evaluationCache;

                evaluationCache = EvaluationInternal(parameters);
                evaluated = true;
                return evaluationCache;
            }
            return EvaluationInternal(parameters);
        }

        public abstract ExpressionPrecedence GetPrecedence();
        public abstract void Accept(IASTVisitor visitor);
        protected abstract Object EvaluationInternal(IDictionary<Object, Object> parameters);
    }
}
