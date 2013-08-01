using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralBoolean : Literal
    {
        public const int TRUE = 1;
        public const int FALSE = 0;

        public LiteralBoolean(bool value)
            : base()
        {
            this.Value = value;
        }

        public bool Value { get; protected set; }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return this.Value ? TRUE : FALSE;
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralBoolean>(this);
        }
    }
}
