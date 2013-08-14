using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CastFunc : FunctionExpression
    {
        public CastFunc(IExpression expr, string typeName,
            IExpression typeInfo1, IExpression typeInfo2)
            : base("CAST", WrapList(expr))
        {
            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName is null");
            this.TypeName = typeName;
            this.TypeInfo1 = typeInfo1;
            this.TypeInfo2 = typeInfo2;
        }

        public IExpression TypeInfo1 { get; protected set; }
        public IExpression TypeInfo2 { get; protected set; }
        public string TypeName { get; protected set; }

        public IExpression GetExpr()
        {
            return base.Arguments.FirstOrDefault();
        }

        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of char has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<CastFunc>(this);
        }
    }
}
