using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class SQLParserDelegate
    {
        private enum SpecialIdentifier
        {
            Rollback,
            SavePoint,
            Truncate
        }

        public static IStatement Parse(string sql, MySQLLexer lexer, string charset)
        {
            try
            {
                IStatement stmt;
                bool isEOF = true;
                MySQLExprParser exprParser = new MySQLExprParser(lexer, charset);
            StmtSwitch:
                switch (lexer.Token())
                {
                    //TODO:之后加入DAL的解析后,要改
                    case MySQLToken.KW_SELECT:
                    case MySQLToken.PUNC_LEFT_PAREN:
                        stmt = new SelectStatement
                        goto StmtSwitch;
                    case MySQLToken.KW_DELETE:
                        goto StmtSwitch;
                    case MySQLToken.KW_INSERT:
                        goto StmtSwitch;
                    case MySQLToken.KW_REPLACE:
                        goto StmtSwitch;
                    case MySQLToken.KW_UPDATE:
                        goto StmtSwitch;
                    case MySQLToken.KW_CALL:
                        goto StmtSwitch;
                    case MySQLToken.KW_SET:
                        goto StmtSwitch;
                    case MySQLToken.KW_SHOW:
                        goto StmtSwitch;
                    case MySQLToken.KW_ALTER:
                    case MySQLToken.KW_CREATE:
                    case MySQLToken.KW_DROP:
                    case MySQLToken.KW_RENAME:
                        goto StmtSwitch;
                    case MySQLToken.KW_RELEASE:
                        goto StmtSwitch;
                    case MySQLToken.IDENTIFIER:
                        goto StmtSwitch;
                    default:
                        throw new SystemException("sql is not a supported statement");
                }
                if (isEOF)
                {
                    while (lexer.Token() == MySQLToken.PUNC_SEMICOLON)
                        lexer.NextToken();
                    if (lexer.Token() != MySQLToken.EOF)
                        throw new SystemException("SQL syntax error!");
                }
                return stmt;
            }
            catch (Exception ex)
            {
                throw new SystemException(BuildErrorMsg(ex, lexer, sql));
            }
        }

        private static bool IsEOFedDDL(IStatement stmt)
        {
            //TODO:需要补充DDL的时候这个函数需要biangeng
        //            if (stmt instanceof DDLStatement) {
        //    if (stmt instanceof DDLCreateIndexStatement) return false;
        //}
            return true;
        }

        private static string BuildErrorMsg(Exception e, MySQLLexer lexer, string sql)
        {
            StringBuilder sb =
                new StringBuilder("You have an error in your SQL syntax; Error occurs around this fragment: ");
            int ch = lexer.CurrentIndex;
            int from = ch - 16;
            if (from < 0) from = 0;
            int to = ch + 9;
            if (to >= sql.Length) to = sql.Length - 1;
            string fragment = sql.Substring(from, to + 1);
            sb.Append('{').Append(fragment).Append('}').Append(". Error cause: " + e.Message);
            return sb.ToString();
        }
    }
}
