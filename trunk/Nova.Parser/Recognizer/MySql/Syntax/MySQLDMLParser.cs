using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MySQLDMLParser : MySQLParser
    {
        protected MySQLExprParser exprParser;

        public MySQLDMLParser(MySQLLexer lexer, MySQLExprParser exprParser)
            : base(lexer)
        {
            this.exprParser = exprParser;
        }

        public SelectStatement Select()
        {
            return new MySQLSelectParser(lexer,exprP
        }

        protected SelectStatement SelectPrimary()
        {

        }

        private ITableReference BuildTableReference(ITableReference reference)
        {
            while (true)
            {
                IExpression on;
                List<string> usage;
                ITableReference temp;
                bool isOut = false, isLeft = true;
                switch (lexer.Token())
                {
                    case MySQLToken.KW_INNER:
                    case MySQLToken.KW_CROSS:
                        lexer.NextToken();
                        goto case MySQLToken.KW_JOIN;
                    case MySQLToken.KW_JOIN:
                        lexer.NextToken();
                        temp = TableFactor();
                }
            }
        }

        private ITableReference TableFactor()
        {
            string alias = String.Empty;
            switch (lexer.Token())
            {
                case MySQLToken.PUNC_LEFT_PAREN:
                    lexer.NextToken();
                    object refence = trs
            }
        }

        private object TrsOrQuery()
        {
            object refence;
            switch (lexer.Token())
            {
                case MySQLToken.KW_SELECT:
                    SelectStatement select = 
            }
        }
    }
}
