using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MySQLExprParser : MySQLParser
    {
        private string charset;
        private MySQLFunctionManager functionManager;

        public MySQLExprParser(MySQLLexer lexer)
        {
        }

        public MySQLExprParser(MySQLLexer lexer, string charset) : this(lexer,MySQLFunctionManager.
        {

        }

        public MySQLExprParser(MySQLLexer lexer, MySQLFunctionManager functionManager, bool cacheEvalRst,
            string charset)
            : base(lexer, cacheEvalRst)
        {
            this.functionManager = functionManager;
            this.charset = charset == null ? DEFAULT_CHARSET : charset;
        }

        public MySQLSelectParser SelectParser { get; set; }
    }
}
