using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MatchExpression : PrimaryExpression
    {
        public enum Modifier
        {
            Default,
            InBooleanMode,
            InNaturalLanguageMode,
            InNaturalLanguageModeWithQueryExpansion,
            WithQueryExpansion
        }

        public MatchExpression(List<IExpression> columns, IExpression pattern, Modifier modifier)
        {
            this.Columns = columns;
            this.Pattern = pattern;
            this.Modify = modifier;
        }

        public List<IExpression> Columns { get; protected set; }
        public IExpression Pattern { get; protected set; }
        public MatchExpression.Modifier Modify { get; protected set; }

        public override void Accept(IASTVisitor visitor)
        {
            visitor.Visit<MatchExpression>(this);
        }
    }
}
