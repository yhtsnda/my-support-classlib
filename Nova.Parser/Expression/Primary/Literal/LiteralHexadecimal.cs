using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralHexadecimal : Literal
    {
        private byte[] bytes;
        private string charset;
        private char[] strings;
        private int offset;
        private int size;

        public LiteralHexadecimal(string introducer, char[] strings, int offset,
            int size, string charset)
            : base()
        {
            if (strings == null || offset + size > strings.Length)
                throw new ArgumentException("hex text is invalid");
            if(String.IsNullOrEmpty(charset))
                throw new ArgumentException("charset is null");

            this.Introducer = introducer;
            this.charset = charset;
            this.strings = strings;
            this.offset = offset;
            this.size = size;
        }

        public string Introducer { get; protected set; }

        public string GetText()
        {
            return new String(strings, offset, size);
        }

        public void AppendTo(StringBuilder builder)
        {
            builder.Append(strings, offset, size);
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            try
            {
                this.bytes = Util.ParseString.HexString2Bytes(strings, offset, size);
                var encoding = Encoding.GetEncoding(Introducer == null ? 
                    charset : 
                    Introducer.Substring(1));
                return encoding.GetString(this.bytes);
            }
            catch(Exception e){
                throw e;
            }
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralHexadecimal>(this);
        }
    }
}
