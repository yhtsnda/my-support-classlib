using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ParamMarker : PrimaryExpression
    {
        public ParamMarker(int paramIndex)
        {
            this.ParamIndex = paramIndex;
        }

        public int ParamIndex { get; protected set; }

        public override int GetHashCode()
        {
            return this.ParamIndex;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj.GetType().IsAssignableFrom(typeof(ParamMarker)))
            {
                ParamMarker that = (ParamMarker)obj;
                return this.ParamIndex == this.ParamIndex;
            }
            return false;
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return parameters.ElementAtOrDefault(this.ParamIndex);
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ParamMarker>(this);
        }
    }
}
