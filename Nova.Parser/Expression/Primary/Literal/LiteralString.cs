using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class LiteralString : Literal
    {
        public LiteralString(string introducer, string text, bool nchars)
            : base()
        {
            this.Introducer = introducer;
            this.Text = text;
            this.NChars = nchars;
        }

        public string Introducer { get; protected set; }
        public string Text { get; protected set; }
        public bool NChars { get; protected set; }

        public static string GetUnescapedString(string str, bool toUppercase)
        {
            StringBuilder builder = new StringBuilder();
            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; ++i)
            {
                char c = chars[i];
                if (c == '\\')
                {
                    switch (c = chars[++i])
                    {
                        case '0':
                            builder.Append('\0');
                            break;
                        case 'b':
                            builder.Append('\b');
                            break;
                        case 'n':
                            builder.Append('\n');
                            break;
                        case 'r':
                            builder.Append('\r');
                            break;
                        case 't':
                            builder.Append('\t');
                            break;
                        case 'Z':
                            builder.Append((char)26);
                            break;
                        default:
                            builder.Append(c);
                            break;
                    }
                }
                else if (c == '\'')
                {
                    ++i;
                    builder.Append('\'');
                }
                else
                {
                    if (toUppercase && c >= 'a' && c <= 'z')
                        c = (char)((int)c - 32);
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        public string GetUnescapedString()
        {
            return GetUnescapedString(this.Text, false);
        }

        public string GetUnescapedString(bool toUppercase)
        {
            return GetUnescapedString(this.Text, toUppercase);
        }

        public static String GetUnescapedString(string str)
        {
            return GetUnescapedString(str, false);
        }

        protected override object EvaluationInternal(IDictionary<object, object> parameters)
        {
            return GetUnescapedString();
        }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<LiteralString>(this);
        }
    }
}
