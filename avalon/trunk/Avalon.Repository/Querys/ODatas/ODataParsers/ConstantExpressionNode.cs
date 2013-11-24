using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Framework.Querys
{
    public class ConstantExpressionNode : ExpressionNode
    {
        public ConstantExpressionNode(IToken token)
        {
            var text = token.Text;
            switch (token.Type)
            {
                case AvalonQueryLexer.STRING:
                    Value = text.Trim('\'');
                    break;
                case AvalonQueryLexer.INT:
                    Value = Int32.Parse(text);
                    break;
                case AvalonQueryLexer.LONG:
                    Value = Int64.Parse(text);
                    break;
                case AvalonQueryLexer.DOUBLE:
                    Value = Double.Parse(text);
                    break;
                case AvalonQueryLexer.SINGLE:
                    Value = Single.Parse(text);
                    break;
                case AvalonQueryLexer.BOOL:
                    Value = Boolean.Parse(text);
                    break;
                case AvalonQueryLexer.NULL:
                    Value = null;
                    break;
                case AvalonQueryLexer.DATETIME:
                    Value = DateTime.Parse(text);
                    break;
                case AvalonQueryLexer.GUID:
                    Value = Guid.Parse(text);
                    break;
                case AvalonQueryLexer.BYTE:
                    Value = Byte.Parse(text);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public object Value { get; private set; }
    }
}
