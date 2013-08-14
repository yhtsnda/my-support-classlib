using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralNumber<TNumber> : Literal where TNumber : struct
    {
        public LiteralNumber(TNumber number)
            : base()
        {
            this.Number = number;
        }

        public TNumber Number { get; protected set; }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return this.Number;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralNumber<TNumber>>(this);
        }
    }
}
