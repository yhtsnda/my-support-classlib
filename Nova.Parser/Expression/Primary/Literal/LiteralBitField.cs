using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralBitField : Literal
    {
        /// <param name="introducer">e.g. "_latin1"</param>
        /// <param name="bitText">e.g. "01010"</param>
        public LiteralBitField(string introducer, string bitText)
            : base()
        {
            if (String.IsNullOrEmpty(bitText))
                throw new ArgumentNullException("bitText");
            this.Introducer = introducer;
            this.Text = bitText;
        }

        public string Introducer { get; protected set; }
        public string Text { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralBitField>(this);
        }
    }
}
