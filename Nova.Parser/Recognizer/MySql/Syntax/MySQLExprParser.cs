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
            : this(lexer, MySQLFunctionManager.Instance, true, DEFAULT_CHARSET)
        {

        }

        public MySQLExprParser(MySQLLexer lexer, string charset)
            : this(lexer, MySQLFunctionManager.Instance, true, charset)
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

        public IExpression Expression()
        {
            MySQLToken token = lexer.Token();
            if (token == null)
                token = lexer.NextToken();
            if (token == MySQLToken.EOF)
                throw new ArgumentException("unexpected EOF");
            
            IExpression left =
        }

        private IExpression LogicalOrExpression()
        {
        }

        private IExpression LogicalAndExpression()
        {
        }

        private IExpression LogicalNotExpression()
        {
        }

        private IExpression LogicalXOrExpression()
        {
        }
    }
}
