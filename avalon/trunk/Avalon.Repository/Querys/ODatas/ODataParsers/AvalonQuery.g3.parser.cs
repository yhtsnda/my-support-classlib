using System;
namespace Avalon.Framework.Querys
{
    partial class AvalonQueryParser
    {
        public override void ReportError(Antlr.Runtime.RecognitionException e)
        {
            throw new QueryParseException(String.Format("OData 语法错误，在 “ {0} ”处发生错误。", e.Token.Text));
        }
    }
}
