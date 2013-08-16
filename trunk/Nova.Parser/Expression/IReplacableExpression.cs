using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public interface IReplacableExpression : IExpression
    {
        LiteralBoolean BOOL_FALSE = new LiteralBoolean(false);
        void SetReplaceExpr(IExpression replaceExpr);
        void ClearReplaceExpr();
    }
}
