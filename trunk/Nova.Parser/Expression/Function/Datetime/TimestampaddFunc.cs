using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class TimestampaddFunc : FunctionExpression
    {
        public IntervalPrimary.UnitType Unit { get; protected set; }

        public TimestampaddFunc(IntervalPrimary.UnitType unit, List<Expression> arguments)
            : base("TIMESTAMPADD", arguments)
        {
            this.Unit = unit;
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of Timestampadd has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<TimestampaddFunc>(this);
        }
    }
}
