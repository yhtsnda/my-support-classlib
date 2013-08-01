using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class AliasableTableReference : ITableReference
    {
        public AliasableTableReference(string alias)
        {
            this.Alias = alias;
        }

        public string Alias { get; protected set; }
        public string AliasUpEscape { get; protected set; }

        public string GetAliasUnescapeUppercase()
        {
            if (String.IsNullOrEmpty(this.Alias))
                return this.Alias;
            if (!String.IsNullOrEmpty(this.AliasUpEscape))
                return this.AliasUpEscape;

            switch (this.Alias[0])
            {
                case '`':
                    return this.AliasUpEscape = Identifier.UnescapeName(this.Alias, true);
                case '\'':
                    return this.AliasUpEscape = LiteralString.GetUnescapedString(
                        this.Alias.Substring(1, this.Alias.Length - 1), true);
                case '_':
                    int index = -1;
                    for (int i = 1; i < this.Alias.Length; ++i)
                    {
                        if (this.Alias[i] == '\'')
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index >= 0)
                    {
                        LiteralString st = new LiteralString(Alias.Substring(0, index), 
                            Alias.Substring(index + 1, Alias.Length - 1), false);
                        return AliasUpEscape = st.GetUnescapedString(true);
                    }
                    return String.Empty;
                default:
                    return AliasUpEscape = Alias.ToUpper();
            }
        }

        public abstract bool IsSingleTable();
        public abstract void RemoveLastConditionElement();
        public abstract void GetPrecedence();
        public abstract void Accept(IASTVisitor visitor);
    }
}
