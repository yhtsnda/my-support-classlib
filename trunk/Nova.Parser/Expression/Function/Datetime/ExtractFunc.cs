using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ExtractFunc : FunctionExpression
    {
        public IntervalPrimary.UnitType Unit { get; protected set; }

        public ExtractFunc(IntervalPrimary.UnitType unit, IExpression date)
            : base("EXTRACT", WrapList(date))
        {
            this.Unit = unit;
        }

        public IExpression GetDate()
        {
            return Arguments.FirstOrDefault();
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of extract has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ExtractFunc>(this);
        }
    }
}
