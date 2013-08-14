using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class FunctionExpression : PrimaryExpression
    {
        protected string FunctionName { get; protected set; }
        protected List<IExpression> Arguments { get; protected set; }

        public FunctionExpression(string functionName, List<IExpression> arguments)
            : base()
        {
            this.FunctionName = functionName;
            this.Arguments = arguments;
        }

        public abstract FunctionExpression ConstructFunction(List<IExpression> arguments);

        public void init()
        {
        }

        public override IExpression SetCacheEvalRst(bool cacheEvalRst)
        {
            return base.SetCacheEvalRst(cacheEvalRst);
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<FunctionExpression>(this);
        }

        protected static List<IExpression> WrapList(IExpression expr)
        {
            List<IExpression> list = new List<IExpression>(1);
            list.Add(expr);
            return list;
        }
    }
}
