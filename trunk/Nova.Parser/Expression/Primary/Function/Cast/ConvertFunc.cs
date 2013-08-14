using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class ConvertFunc : FunctionExpression
    {
        public ConvertFunc(IExpression arg, string transcodeName)
            : base("CONVERT", WrapList(arg))
        {
            if (String.IsNullOrEmpty(transcodeName))
                throw new ArgumentNullException("transcodeName is null");
            this.TranscodeName = transcodeName;
        }

        public string TranscodeName { get; protected set; }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException();
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<ConvertFunc>(this);
        }
    }
}
