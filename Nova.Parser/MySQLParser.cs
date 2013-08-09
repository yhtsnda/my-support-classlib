using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public abstract class MySQLParser
    {
        private enum SpecialIdentifier
        {
            Global,
            Local,
            Session
        }

        public const string DEFAULT_CHARSET = "utf-8";
        protected MySQLLexer lexer;
        protected bool cacheEvalRst;

        public MySQLParser(MySQLLexer lexer)
            : this(lexer, true)
        {
        }

        public MySQLParser(MySQLLexer lexer, bool cacheEvalRst)
        {
            this.lexer = lexer;
            this.cacheEvalRst = cacheEvalRst;
        }

        protected int Match(params MySQLToken[] expectToken)
        {
            if (expectToken == null || expectToken.Length <= 0)
                throw new ArgumentException("at least one expect token");
            MySQLToken token = lexer.Token();
            for (int i = 0; i < expectToken.Length; ++i)
            {
                if (token == expectToken[i])
                {
                    if (token != MySQLToken.EOF || i < expectToken.Length - 1)
                    {
                        lexer.NextToken();
                    }
                    return i;
                }
            }
            throw new ArgumentException("expect " + expectToken);
        }

        public Identifier Identifier()
        {
            if (lexer.Token() == null)
                lexer.NextToken();

            Identifier id;
            switch (lexer.Token())
            {
                case MySQLToken.OP_ASTERISK:
                    lexer.NextToken();
                    Wildcard wc = new Wildcard(null);
                    wc.SetCacheEvalRst(cacheEvalRst);
                    return wc;
                case MySQLToken.IDENTIFIER:
                    id = new Identifier(null, lexer.StringValue, lexer.StringValueUppercase);
                    id.SetCacheEvalRst(cacheEvalRst);
                    lexer.NextToken();
                    break;
                default:
                    throw new ArgumentException("expect id or * after '.'");
            }

            for (; lexer.Token() == MySQLToken.PUNC_DOT; )
            {
                switch (lexer.Token())
                {
                    case MySQLToken.OP_ASTERISK:
                        lexer.NextToken();
                        Wildcard wc = new Wildcard(id);
                        wc.SetCacheEvalRst(cacheEvalRst);
                        return wc;
                    case MySQLToken.IDENTIFIER:
                        id = new Identifier(id, lexer.StringValue, lexer.StringValueUppercase);
                        id.SetCacheEvalRst(cacheEvalRst);
                        lexer.NextToken();
                        break;
                    default:
                        throw new ArgumentException("expect id or * after '.'");
                }
            }
            return id;
        }

        public SysVarPrimary SystemVariable()
        {
            SysVarPrimary sys;
            VariableScope scope = VariableScope.Session;
            string str = lexer.StringValue;
            string strUp = lexer.StringValueUppercase;
            Match(MySQLToken.SYS_VAR);
            SpecialIdentifier si = SpecialIdentifier.Global;
            if (Enum.TryParse<SpecialIdentifier>(strUp, out si))
            {
                switch (si)
                {
                    case SpecialIdentifier.Global:
                        scope = VariableScope.Global;
                        break;
                    case SpecialIdentifier.Session:
                    case SpecialIdentifier.Local:
                        Match(MySQLToken.PUNC_DOT);
                        str = lexer.StringValue;
                        strUp = lexer.StringValueUppercase;
                        Match(MySQLToken.IDENTIFIER);
                        sys = new SysVarPrimary(scope, str, strUp);
                        sys.SetCacheEvalRst(cacheEvalRst);
                        return sys;
                }
            }
            sys = new SysVarPrimary(scope, str, strUp);
            sys.SetCacheEvalRst(cacheEvalRst);
            return sys;
        }
        
        
    }
}
