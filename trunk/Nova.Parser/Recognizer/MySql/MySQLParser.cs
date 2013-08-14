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

        protected ParamMarker CreateParam(int index)
        {
            ParamMarker param = new ParamMarker(index);
            param.SetCacheEvalRst(cacheEvalRst);
            return param;
        }

        protected PlaceHolder CreatePlaceholder(string str, string strup)
        {
            PlaceHolder ph = new PlaceHolder(str, strup);
            ph.SetCacheEvalRst(cacheEvalRst);
            return ph;
        }

        protected LimitFragment Limit()
        {
            if (lexer.Token() != MySQLToken.KW_LIMIT)
                return null;

            int paramIndex1, paramIndex2;
            Decimal num1;

            switch (lexer.NextToken())
            {
                case MySQLToken.LITERAL_NUM_PURE_DIGIT:
                    num1 = lexer.IntegerValue();
                    switch (lexer.NextToken())
                    {
                        case MySQLToken.PUNC_COMMA:
                            switch (lexer.NextToken())
                            {
                                case MySQLToken.LITERAL_NUM_PURE_DIGIT:
                                    decimal num2 = lexer.IntegerValue();
                                    lexer.NextToken();
                                    return new LimitFragment(num1, num2);
                                case MySQLToken.QUESTION_MARK:
                                    paramIndex1 = lexer.ParamIndex;
                                    lexer.NextToken();
                                    return new LimitFragment(num1, CreateParam(paramIndex1));
                                default:
                                    throw new ArgumentException("expect digit or ? after , for limit");
                            }
                        case MySQLToken.IDENTIFIER:
                            if ("OFFSET".Equals(lexer.StringValueUppercase))
                            {
                                switch (lexer.NextToken())
                                {
                                    case MySQLToken.LITERAL_NUM_PURE_DIGIT:
                                        decimal num2 = lexer.IntegerValue();
                                        lexer.NextToken();
                                        return new LimitFragment(num2, num1);
                                    case MySQLToken.QUESTION_MARK:
                                        paramIndex1 = lexer.ParamIndex;
                                        lexer.NextToken();
                                        return new LimitFragment(CreateParam(paramIndex1), num1);
                                    default:
                                        throw new ArgumentException("expect digit or ? after , for limit");
                                }
                            }
                            break;
                    }
                    return new LimitFragment(new decimal(0), num1);
                case MySQLToken.QUESTION_MARK:
                    paramIndex1 = lexer.ParamIndex;
                    switch (lexer.NextToken())
                    {
                        case MySQLToken.PUNC_COMMA:
                            switch (lexer.NextToken())
                            {
                                case MySQLToken.LITERAL_NUM_PURE_DIGIT:
                                    num1 = lexer.IntegerValue();
                                    lexer.NextToken();
                                    return new LimitFragment(CreateParam(paramIndex1), num1);
                                case MySQLToken.QUESTION_MARK:
                                    paramIndex2 = lexer.ParamIndex;
                                    lexer.NextToken();
                                    return new LimitFragment(CreateParam(paramIndex1), CreateParam(paramIndex2));
                                default:
                                    throw new ArgumentException("expect digit or ? after , for limit");
                            }
                        case MySQLToken.IDENTIFIER:
                            if ("OFFSET".Equals(lexer.StringValueUppercase))
                            {
                                switch (lexer.NextToken())
                                {
                                    case MySQLToken.LITERAL_NUM_PURE_DIGIT:
                                        decimal num2 = lexer.IntegerValue();
                                        lexer.NextToken();
                                        return new LimitFragment(num2, CreateParam(paramIndex1));
                                    case MySQLToken.QUESTION_MARK:
                                        paramIndex2 = lexer.ParamIndex;
                                        lexer.NextToken();
                                        return new LimitFragment(CreateParam(paramIndex2), CreateParam(paramIndex1));
                                    default:
                                        throw new ArgumentException("expect digit or ? after , for limit");
                                }
                            }
                            break;
                    }
                    return new LimitFragment(new decimal(0), CreateParam(paramIndex1));
                default:
                    throw new ArgumentException("expect digit or ? after , for limit");
            }
        }

        protected int EqualsIdentifier(params string[] expectTextUppercases)
        {
            if (lexer.Token() == MySQLToken.IDENTIFIER)
            {
                string id = lexer.StringValueUppercase;
                for (int i = 0; i < expectTextUppercases.Length; ++i)
                {
                    if (expectTextUppercases[i].Equals(id))
                        return i;
                }
            }
            return -1;
        }

        protected int MatchIdentifier(params string[] expectTextUppercase)
        {
            if (expectTextUppercase == null || expectTextUppercase.Length <= 0)
                throw new ArgumentException("at least one expect token");
            if (lexer.Token() != MySQLToken.IDENTIFIER)
                throw new ArgumentException("expect an id");

            string id = lexer.StringValueUppercase;
            for (int i = 0; i < expectTextUppercase.Length; ++i)
            {
                if (id == null ? expectTextUppercase[i] == null : id.Equals(expectTextUppercase[i]))
                {
                    lexer.NextToken();
                    return i;
                }
            }
            throw new ArgumentException("expect " + expectTextUppercase.ToString());
        }

        protected int Match(params MySQLToken[] expectToken)
        {
            if(expectToken == null || expectToken.Length <= 0)
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
            throw new ArgumentException("expect " + expectToken.ToString());
        }
    }
}
