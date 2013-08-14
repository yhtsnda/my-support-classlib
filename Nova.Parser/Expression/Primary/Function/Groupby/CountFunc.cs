using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CountFunc : FunctionExpression
    {
        public bool Distinct { get; protected set; }

        public CountFunc(List<IExpression> arguments)
            : base("COUNT", arguments)
        {
            this.Distinct = true;
        }

        public CountFunc(IExpression arg)
            : base("COUNT", WrapList(arg))
        {
            this.Distinct = false;
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            return new CountFunc(arguments);
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<CountFunc>(this);
        }
    }
}
