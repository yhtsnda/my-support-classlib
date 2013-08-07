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

        public Identifier identifier()
        {
        }
    }
}
