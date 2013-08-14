using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class CharFunc : FunctionExpression
    {
        public string Charset { get; protected set; }

        public CharFunc(List<IExpression> args, string charset)
            : base("CHAR", args)
        {
            this.Charset = charset;
        }
 
        public override FunctionExpression ConstructFunction(List<IExpression> arguments)
        {
            throw new NotImplementedException("function of char has special arguments");
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<CharFunc>(this);
        }
    }
}
