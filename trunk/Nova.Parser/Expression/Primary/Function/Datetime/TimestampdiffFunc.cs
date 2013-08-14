using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimestampdiffFunc : FunctionExpression
    {
        public IntervalPrimary.UnitType Unit { get; protected set; }

        public TimestampdiffFunc(IntervalPrimary.UnitType unit, List<Expression> arguments)
            : base("TIMESTAMPADD", arguments)
        {
            this.Unit = unit;
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of Timestampdiff has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<TimestampdiffFunc>(this);
        }
    }
}
