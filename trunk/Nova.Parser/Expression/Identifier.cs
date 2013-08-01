using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class Identifier : PrimaryExpression
    {
        public Identifier(Identifier parent, string idText, string idTextUp)
        {
            this.parent = parent;
            this.idText = idText;
            this.idTextUpUnescape = UnescapeName(idTextUp, false);
        }

        public Identifier(Identifier parent, string idText)
            : this(parent, idText, idText.ToUpper())
        {
            
        }

        protected Identifier parent;
        protected string idText;
        protected string idTextUpUnescape;

        public static string UnescapeName(string name, bool toUpper)
        {
            return String.Empty;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void Accept(IASTVisitor visitor)
        {
            base.Accept(visitor);
        }
    }
}
