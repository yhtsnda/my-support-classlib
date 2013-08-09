using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace Nova.Parser
{
    public class MySQLLexer
    {
        private static int C_STYLE_COMMENT_VERSION = 50599;
        private const byte EOI = 0x1A;
        private MySQLToken token;
        private MySQLToken? tokenCache, tokenCache2;
        private int paramIndex = 0;
        private string stringValue, stringValueUppercase;

        protected char ch;
        protected char[] sql, sbuf;
        protected int eofIndex, curIndex= -1;
        protected static ThreadLocal<char[]> sbufRef = new ThreadLocal<char[]>();
        protected bool inCStyleComment, inCStyleCommentIgnore;
        protected int offsetCache, sizeCache;

        public int ParamIndex { get { return paramIndex; } }
        public string StringValueUppercase { get { return stringValueUppercase; } }
        public string StringValue { get { return stringValue; } }

        public MySQLLexer(char[] sql)
        {
            if ((this.sbuf = sbufRef.Value) == null)
            {
                this.sbuf = new char[1024];
                sbufRef.Value = this.sbuf;
            }

            if (CharTypes.IsWhitespace(sql[sql.Length - 1]))
                this.sql = sql;
            else
            {
                this.sql = new char[sql.Length + 1];
                Array.ConstrainedCopy(sql, 0, this.sql, 0, sql.Length);
            }
            this.eofIndex = this.sql.Length - 1;
            this.sql[this.eofIndex] = (char)MySQLLexer.EOI;
        }

        protected bool IsEof()
        {
            return curIndex >= eofIndex;
        }

        protected bool HasChars(int howMany)
        {
            return curIndex + howMany <= eofIndex;
        }

        protected void PutChar(char ch, int index)
        {
            if (index >= sbuf.Length)
            {
                char[] newsbuf = new char[sbuf.Length * 2];
                Array.ConstrainedCopy(sbuf, 0, newsbuf, 0, sbuf.Length);
                sbuf = newsbuf;
            }
            sbuf[index] = ch;
        }

        protected char ScanChar()
        {
            return ch = sql[++curIndex];
        }

        protected char ScanChar(int skip)
        {
            return ch = sql[curIndex += skip];
        }

        protected void ScanHexaDecimal(bool quoteMode)
        {
            this.offsetCache = this.curIndex;
            this.sizeCache = this.curIndex - this.offsetCache;
            if (quoteMode)
            {
                if (ch != '\'')
                    throw new ArgumentException("invalid char for hex: " + ch);
                ScanChar();
            }
            else if (CharTypes.IsIdentifierChar(ch))
            {
                ScanIdentifierFromNumber(offsetCache - 2, sizeCache + 2);
                return;
            }
            this.token = MySQLToken.LITERAL_HEX;
        }

        protected void ScanBitField(bool quoteMode)
        {
            offsetCache = curIndex;
            for (; ch == '0' || ch == '1'; ScanChar()) ;
            sizeCache = curIndex - offsetCache;

            if (quoteMode)
            {
                if (ch != '\'')
                    throw new Exception("invalid char for bit: " + ch);
                ScanChar();
            }
            else if (CharTypes.IsIdentifierChar(ch))
            {
                ScanIdentifierFromNumber(offsetCache - 2, sizeCache + 2);
                return;
            }
            token = MySQLToken.LITERAL_BIT;
            stringValue = new string(sql, offsetCache, sizeCache);
        }

        protected void ScanNumber()
        {
            offsetCache = curIndex;
            sizeCache = 1;

            bool fstDot = ch == '.';
            bool dot = fstDot;
            bool sign = false;
            int state = fstDot ? 1 : 0;

            for (; ScanChar() != MySQLLexer.EOI; ++sizeCache)
            {
                switch (state)
                {
                    case 0:
                        if (CharTypes.IsDigit(ch))
                        {
                        }
                        else if (ch == '.')
                        {
                            dot = true;
                            state = 1;
                        }
                        else if(ch == 'e' || ch == 'E')
                        {
                            state = 3;
                        }
                        else if (CharTypes.IsIdentifierChar(ch))
                        {
                            ScanIdentifierFromNumber(offsetCache, sizeCache);
                            return;
                        }
                        else
                        {
                            token = MySQLToken.LITERAL_NUM_PURE_DIGIT;
                            return;
                        }
                        break;
                    case 1:
                        if (CharTypes.IsDigit(ch))
                            state = 2;
                        else if (ch == 'e' || ch == 'E')
                            state = 3;
                        else if (CharTypes.IsIdentifierChar(ch) && fstDot)
                        {
                            sizeCache = 1;
                            ch = sql[curIndex = offsetCache + 1];
                            token = MySQLToken.PUNC_DOT;
                            return;
                        }
                        else
                        {
                            token = MySQLToken.LITERAL_NUM_MIX_DIGIT;
                            return;
                        }
                        break;
                    case 2:
                        if (CharTypes.IsDigit(ch))
                        {
                        }
                        else if (ch == 'e' || ch == 'E')
                        {
                            state = 3;
                        }
                        else if (CharTypes.IsIdentifierChar(ch) && fstDot)
                        {
                            sizeCache = 1;
                            ch = sql[curIndex = offsetCache + 1];
                            token = MySQLToken.PUNC_DOT;
                            return;
                        }
                        else
                        {
                            token = MySQLToken.LITERAL_NUM_MIX_DIGIT;
                            return;
                        }
                        break;
                    case 3:
                        if (CharTypes.IsDigit(ch))
                        {
                            state = 5;
                        }
                        else if (ch == '+' || ch == '-')
                        {
                            sign = true;
                            state = 4;
                        }
                        else if (fstDot)
                        {
                            sizeCache = 1;
                            ch = sql[curIndex = offsetCache + 1];
                            token = MySQLToken.PUNC_DOT;
                            return;
                        }
                        else if (!dot)
                        {
                            if (CharTypes.IsIdentifierChar(ch))
                            {
                                ScanIdentifierFromNumber(offsetCache, sizeCache);
                            }
                            else
                            {
                                UpdateStringValue(sql, offsetCache, sizeCache);
                                MySQLToken tok = MySQLKeywords.Instance.GetKeyword(stringValueUppercase);
                                token = tok == null ? MySQLToken.IDENTIFIER : tok;
                            }
                            return;
                        }
                        else
                        {
                            throw new ArgumentException("invalid char after '.' and 'e' for as part of number: " + ch);
                        }
                        break;
                    case 4:
                        if (CharTypes.IsDigit(ch))
                        {
                            state = 5;
                            break;
                        }
                        else if (fstDot)
                        {
                            sizeCache = 1;
                            ch = sql[curIndex = offsetCache + 1];
                            token = MySQLToken.PUNC_DOT;
                        }
                        else if (!dot)
                        {
                            ch = sql[--curIndex];
                            --sizeCache;
                            UpdateStringValue(sql, offsetCache, sizeCache);
                            MySQLToken tok = MySQLKeywords.Instance.GetKeyword(stringValueUppercase);
                            token = tok == null ? MySQLToken.IDENTIFIER : tok;
                        }
                        else
                        {
                            throw new ArgumentException("expect digit char after SIGN for 'e': " + ch);
                        }
                        return;
                    case 5:
                        if (CharTypes.IsDigit(ch))
                        {
                            break;
                        }
                        else if (CharTypes.IsIdentifierChar(ch))
                        {
                            if (fstDot)
                            {
                                sizeCache = 1;
                                ch = sql[curIndex = offsetCache + 1];
                                token = MySQLToken.PUNC_DOT;
                            }
                            else if (!dot)
                            {
                                if (sign)
                                {
                                    ch = sql[curIndex = offsetCache];
                                    ScanIdentifierFromNumber(curIndex, 0);
                                }
                                else
                                {
                                    ScanIdentifierFromNumber(offsetCache, sizeCache);
                                }
                            }
                            else
                            {
                                token = MySQLToken.LITERAL_NUM_MIX_DIGIT;
                            }
                        }
                        else
                        {
                            token = MySQLToken.LITERAL_NUM_MIX_DIGIT;
                        }
                        return;
                }
            }

            switch (state)
            {
                case 0:
                    token = MySQLToken.LITERAL_NUM_PURE_DIGIT;
                    return;
                case 1:
                    if (fstDot)
                    {
                        token = MySQLToken.PUNC_DOT;
                        return;
                    }
                    break;
                case 2:
                case 5:
                    token = MySQLToken.LITERAL_NUM_MIX_DIGIT;
                    return;
                case 3:
                    if (fstDot)
                    {
                        sizeCache = 1;
                        ch = sql[curIndex = offsetCache + 1];
                        token = MySQLToken.PUNC_DOT;
                    }
                    else if (!dot)
                    {
                        UpdateStringValue(sql, offsetCache, sizeCache);
                        MySQLToken tok = MySQLKeywords.Instance.GetKeyword(stringValueUppercase);
                        token = tok == null ? MySQLToken.IDENTIFIER : tok;
                    }
                    else
                    {
                        throw new ArgumentException("expect digit char after SIGN for 'e': " + ch);
                    }
                    return;
                case 4:
                    if (fstDot)
                    {
                        sizeCache = 1;
                        ch = sql[curIndex = offsetCache + 1];
                        token = MySQLToken.PUNC_DOT;
                    }
                    else if (!dot)
                    {
                        ch = sql[--curIndex];
                        --sizeCache;
                        UpdateStringValue(sql, offsetCache, sizeCache);
                        MySQLToken tok = MySQLKeywords.Instance.GetKeyword(stringValueUppercase);
                        token = tok == null ? MySQLToken.IDENTIFIER : tok;
                    }
                    else
                    {
                        throw new ArgumentException("expect digit char after SIGN for 'e': " + ch);
                    }
                    return;
            }
        }

        protected void ScanIdentifier()
        {
            if (ch == '$')
            {
                if (ScanChar() == '{')
                    ScanPlaceHolder();
                else
                    ScanIdentifierFromNumber(curIndex - 1, 1);
            }
            else
                ScanIdentifierFromNumber(curIndex, 0);
        }

        protected void ScanPlaceHolder()
        {
            this.offsetCache = curIndex + 1;
            this.sizeCache = 0;
            for (ScanChar(); ch != '}' && !IsEof(); ++sizeCache)
                ScanChar();
            if (ch == '}')
                ScanChar();
            UpdateStringValue(sql, offsetCache, sizeCache);
            token = MySQLToken.PLACE_HOLDER;
        }

        protected void ScanIdentifierWithAccent()
        {
            offsetCache = curIndex;
            for (; ScanChar() != MySQLLexer.EOI; )
            {
                if (ch == '`' && ScanChar() != '`')
                {
                    break;
                }
            }
            UpdateStringValue(sql, offsetCache, sizeCache = curIndex - offsetCache);
            token = MySQLToken.IDENTIFIER;
        }

        protected void ScanString()
        {
            bool dq = false;
            if (ch == '\''){ }
            else if (ch == '"')
                dq = true;
            else
                throw new ArgumentException("first char must be \" or '");

            offsetCache = curIndex;
            int size = 1;
            sbuf[0] = '\'';
            if (dq)
            {
                while (true)
                {
                    switch (ScanChar())
                    {
                        case '\'':
                            PutChar('\\', size++);
                            PutChar('\'', size++);
                            break;
                        case '\\':
                            PutChar('\\', size++);
                            PutChar(ScanChar(), size++);
                            continue;
                        case '"':
                            if (sql[curIndex + 1] == '"')
                            {
                                PutChar('"', size++);
                                ScanChar();
                                continue;
                            }
                            PutChar('\'', size++);
                            ScanChar();
                            goto label1;
                        default:
                            if (IsEof())
                            {
                                throw new ArgumentException("unclosed string");
                            }
                            PutChar(ch, size++);
                            continue;
                    }
                }
            label1:
                ;
            }
            else
            {
                while (true)
                {
                    switch (ScanChar())
                    {
                        case '\\':
                            PutChar('\\', size++);
                            PutChar(ScanChar(), size++);
                            continue;
                        case '\'':
                            if (sql[curIndex + 1] == '\'')
                            {
                                PutChar('\\', size++);
                                PutChar(ScanChar(), size++);
                                continue;
                            }
                            PutChar('\'', size++);
                            ScanChar();
                            goto label2;
                        default:
                            if (IsEof())
                            {
                                throw new ArgumentException("unclosed string");
                            }
                            PutChar(ch, size++);
                            continue;
                    }
                }
            label2:
                ;
            }
            sizeCache = size;
            stringValue = new String(sbuf, 0, size);
            token = MySQLToken.LITERAL_CHARS;
        }

        protected void ScanUserVeriable()
        {
            if (ch != '@') throw new ArgumentException("first char must be @");
            offsetCache = curIndex;
            sizeCache = 1;

            bool dq = false;
            switch (ScanChar())
            {
                case '"':
                    dq = true;
                    break;
                case '\'':
                    for (++sizeCache; ; ++sizeCache)
                    {
                        switch (ScanChar())
                        {
                            case '\\':
                                ++sizeCache;
                                ScanChar();
                                break;
                            case '"':
                                if (dq)
                                {
                                    ++sizeCache;
                                    if (ScanChar() == '"')
                                    {
                                        break;
                                    }
                                    goto label3;
                                }
                                break;
                            case '\'':
                                if (!dq)
                                {
                                    ++sizeCache;
                                    if (ScanChar() == '\'')
                                    {
                                        break;
                                    }
                                    goto label3;
                                }
                                break;
                        }
                    }
                label3:
                    break;
                case '`':
                loop1: for (++sizeCache; ; ++sizeCache)
                    {
                        switch (ScanChar())
                        {
                            case '`':
                                ++sizeCache;
                                if (ScanChar() == '`')
                                {
                                    break;
                                }
                                goto label4;
                        }
                    }
                label4:
                    break;
                default:
                    for (; CharTypes.IsIdentifierChar(ch) || ch == '.'; ++sizeCache)
                    {
                        ScanChar();
                    }
                    break;
            }

            stringValue = new String(sql, offsetCache, sizeCache);
            token = MySQLToken.USR_VAR;
        }

        protected void ScanSystemVeriable()
        {
            if (ch != '@' || sql[curIndex + 1] != '@') throw err("first char must be @@");
            offsetCache = curIndex + 2;
            sizeCache = 0;
            ScanChar(2);
            if (ch == '`')
            {
                for (++sizeCache; ; ++sizeCache)
                {
                    if (ScanChar() == '`')
                    {
                        ++sizeCache;
                        if (ScanChar() != '`')
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                for (; CharTypes.IsIdentifierChar(ch); ++sizeCache)
                {
                    ScanChar();
                }
            }
            UpdateStringValue(sql, offsetCache, sizeCache);
            token = MySQLToken.SYS_VAR;
        }

        protected void SkipSeparator()
        {
            for (; !IsEof(); )
            {
                for (; CharTypes.IsWhitespace(ch); ScanChar()) ;

                switch (ch)
                {
                    case '#': // MySQL specified
                        for (; ScanChar() != '\n'; )
                        {
                            if (IsEof())
                            {
                                return;
                            }
                        }
                        ScanChar();
                        continue;
                    case '/':
                        if (HasChars(2) && '*' == sql[curIndex + 1])
                        {
                            bool commentSkip;
                            if ('!' == sql[curIndex + 2])
                            {
                                ScanChar(3);
                                inCStyleComment = true;
                                inCStyleCommentIgnore = false;
                                commentSkip = false;
                                // MySQL use 5 digits to indicate version. 50508 means MySQL 5.5.8
                                if (HasChars(5)
                                    && CharTypes.IsDigit(ch)
                                    && CharTypes.IsDigit(sql[curIndex + 1])
                                    && CharTypes.IsDigit(sql[curIndex + 2])
                                    && CharTypes.IsDigit(sql[curIndex + 3])
                                    && CharTypes.IsDigit(sql[curIndex + 4]))
                                {
                                    int version = ch - '0';
                                    version *= 10;
                                    version += sql[curIndex + 1] - '0';
                                    version *= 10;
                                    version += sql[curIndex + 2] - '0';
                                    version *= 10;
                                    version += sql[curIndex + 3] - '0';
                                    version *= 10;
                                    version += sql[curIndex + 4] - '0';
                                    ScanChar(5);
                                    if (version > C_STYLE_COMMENT_VERSION)
                                    {
                                        inCStyleCommentIgnore = true;
                                    }
                                }
                                SkipSeparator();
                            }
                            else
                            {
                                ScanChar(2);
                                commentSkip = true;
                            }

                            if (commentSkip)
                            {
                                for (int state = 0; !IsEof(); ScanChar())
                                {
                                    if (state == 0)
                                    {
                                        if ('*' == ch)
                                        {
                                            state = 1;
                                        }
                                    }
                                    else
                                    {
                                        if ('/' == ch)
                                        {
                                            ScanChar();
                                            break;
                                        }
                                        else if ('*' != ch)
                                        {
                                            state = 0;
                                        }
                                    }
                                }
                                continue;
                            }
                        }
                        return;
                    case '-':
                        if (HasChars(3) && '-' == sql[curIndex + 1] && CharTypes.IsWhitespace(sql[curIndex + 2]))
                        {
                            ScanChar(3);
                            for (; !IsEof(); ScanChar())
                            {
                                if ('\n' == ch)
                                {
                                    ScanChar();
                                    break;
                                }
                            }
                            continue;
                        }
                        break;
                    default:
                        return;
                }
            }
        }

        protected void UpdateStringValue(char[] src, int srcOffset, int len)
        {
            this.stringValue = new string(src, srcOffset, len);
            int end = srcOffset + len;
            bool lowerCase = false;
            int srcIndex = srcOffset;
            int hash = 0;

            for (; srcIndex < end; ++srcIndex)
            {
                char c = src[srcIndex];
                if (c >= 'a' && c <= 'z')
                {
                    lowerCase = true;
                    if (srcIndex > srcOffset)
                    {
                        Array.ConstrainedCopy(src, srcOffset, sbuf, 0, srcIndex - srcOffset);
                    }
                    break;
                }
                hash = 31 * hash + c;
            }

            if (lowerCase)
            {
                for (int destIndex = srcIndex - srcOffset; destIndex < len; ++destIndex)
                {
                    char c = src[srcIndex++];
                    hash = 31 * hash + c;
                    if (c >= 'a' && c <= 'z')
                    {
                        sbuf[destIndex] = (char)(c - 32);
                        hash -= 32;
                    }
                    else
                    {
                        sbuf[destIndex] = c;
                    }
                }
                stringValueUppercase = new String(sbuf, 0, len);
            }
            else
            {
                stringValueUppercase = new String(sbuf, srcOffset, len);
            }
        }

        private MySQLToken NextTokenInternal()
        {
            switch (ch)
            {
                case '0':
                    switch (sql[curIndex + 1])
                    {
                        case 'x':
                            ScanChar(2);
                            ScanHexaDecimal(false);
                            return token;
                        case 'b':
                            ScanChar(2);
                            ScanBitField(false);
                            return token;
                    }
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ScanNumber();
                    return token;
                case '.':
                    if (CharTypes.IsDigit(sql[curIndex + 1]))
                    {
                        ScanNumber();
                    }
                    else
                    {
                        ScanChar();
                        token = MySQLToken.PUNC_DOT;
                    }
                    return token;
                case '\'':
                case '"':
                    ScanString();
                    return token;
                case 'n':
                case 'N':
                    if (sql[curIndex + 1] == '\'')
                    {
                        ScanChar();
                        ScanString();
                        token = MySQLToken.LITERAL_NCHARS;
                        return token;
                    }
                    ScanIdentifier();
                    return token;
                case 'x':
                case 'X':
                    if (sql[curIndex + 1] == '\'')
                    {
                        ScanChar(2);
                        ScanHexaDecimal(true);
                        return token;
                    }
                    ScanIdentifier();
                    return token;
                case 'b':
                case 'B':
                    if (sql[curIndex + 1] == '\'')
                    {
                        ScanChar(2);
                        ScanBitField(true);
                        return token;
                    }
                    ScanIdentifier();
                    return token;
                case '@':
                    if (sql[curIndex + 1] == '@')
                    {
                        ScanSystemVariable();
                        return token;
                    }
                    ScanUserVariable();
                    return token;
                case '?':
                    ScanChar();
                    token = MySQLToken.QUESTION_MARK;
                    ++paramIndex;
                    return token;
                case '(':
                    ScanChar();
                    token = MySQLToken.PUNC_LEFT_PAREN;
                    return token;
                case ')':
                    ScanChar();
                    token = MySQLToken.PUNC_RIGHT_PAREN;
                    return token;
                case '[':
                    ScanChar();
                    token = MySQLToken.PUNC_LEFT_BRACKET;
                    return token;
                case ']':
                    ScanChar();
                    token = MySQLToken.PUNC_RIGHT_BRACKET;
                    return token;
                case '{':
                    ScanChar();
                    token = MySQLToken.PUNC_LEFT_BRACE;
                    return token;
                case '}':
                    ScanChar();
                    token = MySQLToken.PUNC_RIGHT_BRACE;
                    return token;
                case ',':
                    ScanChar();
                    token = MySQLToken.PUNC_COMMA;
                    return token;
                case ';':
                    ScanChar();
                    token = MySQLToken.PUNC_SEMICOLON;
                    return token;
                case ':':
                    if (sql[curIndex + 1] == '=')
                    {
                        ScanChar(2);
                        token = MySQLToken.OP_ASSIGN;
                        return token;
                    }
                    ScanChar();
                    token = MySQLToken.PUNC_COLON;
                    return token;
                case '=':
                    ScanChar();
                    token = MySQLToken.OP_EQUALS;
                    return token;
                case '~':
                    ScanChar();
                    token = MySQLToken.OP_TILDE;
                    return token;
                case '*':
                    if (inCStyleComment && sql[curIndex + 1] == '/')
                    {
                        inCStyleComment = false;
                        inCStyleCommentIgnore = false;
                        ScanChar(2);
                        token = MySQLToken.PUNC_C_STYLE_COMMENT_END;
                        return token;
                    }
                    ScanChar();
                    token = MySQLToken.OP_ASTERISK;
                    return token;
                case '-':
                    ScanChar();
                    token = MySQLToken.OP_MINUS;
                    return token;
                case '+':
                    ScanChar();
                    token = MySQLToken.OP_PLUS;
                    return token;
                case '^':
                    ScanChar();
                    token = MySQLToken.OP_CARET;
                    return token;
                case '/':
                    ScanChar();
                    token = MySQLToken.OP_SLASH;
                    return token;
                case '%':
                    ScanChar();
                    token = MySQLToken.OP_PERCENT;
                    return token;
                case '&':
                    if (sql[curIndex + 1] == '&')
                    {
                        ScanChar(2);
                        token = MySQLToken.OP_LOGICAL_AND;
                        return token;
                    }
                    ScanChar();
                    token = MySQLToken.OP_AMPERSAND;
                    return token;
                case '|':
                    if (sql[curIndex + 1] == '|')
                    {
                        ScanChar(2);
                        token = MySQLToken.OP_LOGICAL_OR;
                        return token;
                    }
                    ScanChar();
                    token = MySQLToken.OP_VERTICAL_BAR;
                    return token;
                case '!':
                    if (sql[curIndex + 1] == '=')
                    {
                        ScanChar(2);
                        token = MySQLToken.OP_NOT_EQUALS;
                        return token;
                    }
                    ScanChar();
                    token = MySQLToken.OP_EXCLAMATION;
                    return token;
                case '>':
                    switch (sql[curIndex + 1])
                    {
                        case '=':
                            ScanChar(2);
                            token = MySQLToken.OP_GREATER_OR_EQUALS;
                            return token;
                        case '>':
                            ScanChar(2);
                            token = MySQLToken.OP_RIGHT_SHIFT;
                            return token;
                        default:
                            ScanChar();
                            token = MySQLToken.OP_GREATER_THAN;
                            return token;
                    }
                case '<':
                    switch (sql[curIndex + 1])
                    {
                        case '=':
                            if (sql[curIndex + 2] == '>')
                            {
                                ScanChar(3);
                                token = MySQLToken.OP_NULL_SAFE_EQUALS;
                                return token;
                            }
                            ScanChar(2);
                            token = MySQLToken.OP_LESS_OR_EQUALS;
                            return token;
                        case '>':
                            ScanChar(2);
                            token = MySQLToken.OP_LESS_OR_GREATER;
                            return token;
                        case '<':
                            ScanChar(2);
                            token = MySQLToken.OP_LEFT_SHIFT;
                            return token;
                        default:
                            ScanChar();
                            token = MySQLToken.OP_LESS_THAN;
                            return token;
                    }
                case '`':
                    ScanIdentifierWithAccent();
                    return token;
                default:
                    if (CharTypes.IsIdentifierChar(ch))
                    {
                        ScanIdentifier();
                    }
                    else if (IsEof())
                    {
                        token = MySQLToken.EOF;
                        curIndex = eofIndex;
                        //tokenPos = curIndex;
                    }
                    else
                    {
                        throw new ArgumentException("unsupported character: " + ch);
                    }
                    return token;
            }
        }

        private void ScanIdentifierFromNumber(int initOffset, int initSize)
        {
            this.offsetCache = initOffset;
            this.sizeCache = initSize;

            for (; CharTypes.IsIdentifierChar(ch); ++sizeCache)
                ScanChar();
            UpdateStringValue(sql, offsetCache, sizeCache);
            MySQLToken tok = MySQLKeywords.Instance.GetKeyword(stringValueUppercase);
            
            this.token = tok == MySQLToken.NONE ? MySQLToken.IDENTIFIER ? tok;
        }

        public MySQLToken NextToken()
        {
            if (tokenCache2 != null)
            {
                tokenCache2 = null;
                return tokenCache.Value;
            }
            if (tokenCache != null)
            {
                tokenCache = null;
                return token;
            }
            if (token == MySQLToken.EOF)
                throw new ArgumentException("eof for sql is already reached, cannot get new token");

            MySQLToken t;
            do{
                SkipSeparator();
                t = NextTokenInternal();
            }while(inCStyleComment && inCStyleCommentIgnore || MySQLToken.PUNC_C_STYLE_COMMENT_END ==t)
            return t;
        }

        public MySQLToken Token()
        {
            if (tokenCache2 != null)
                return tokenCache2.Value;
            if (tokenCache != null)
                return tokenCache.Value;
            return token;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("MySQLLexer@")
                .Append(GetHashCode())
                .Append("{");
            string sqlLeft = new String(sql, curIndex, sql.Length - curIndex);
            builder.Append("curIndex=")
                  .Append(curIndex)
                  .Append(", ch=")
                  .Append(ch)
                  .Append(", token=")
                  .Append(token)
                  .Append(", sqlLeft=")
                  .Append(sqlLeft)
                  .Append(", sql=")
                  .Append(sql)
                  .Append('}');
            return builder.ToString();
        }
    }
}
