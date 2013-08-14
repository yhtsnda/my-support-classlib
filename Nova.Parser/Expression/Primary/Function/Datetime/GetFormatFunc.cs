using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class GetFormatFunc : FunctionExpression
    {
        public enum FormatType
        {
            Date,
            Time,
            Datetime
        }

        public FormatType FormatType { get; protected set; }

        public GetFormatFunc(FormatType type, IExpression format)
            : base("ADDTIME", WrapList(format))
        {
            this.FormatType = type;
        }

        public IExpression GetFormat()
        {
            return Arguments.FirstOrDefault();
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of GetFormat has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<GetFormatFunc>(this);    
        }
    }
}
