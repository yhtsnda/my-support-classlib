using Antlr.Runtime;
using System;
namespace Avalon.Framework.Querys
{
    partial class AvalonQueryLexer
    {
        public override void ReportError(Antlr.Runtime.RecognitionException e)
        {
            var str = ((ANTLRReaderStream)e.Input).Substring(e.Index, e.Input.Count - e.Index);

            throw new QueryParseException(String.Format("OData 语法错误，在 “ {0} ”处发生错误。", str));
        }
    }
}
