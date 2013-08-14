using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MySQLSelectParser : MySQLDMLParser
    {
        private enum SpecialIdentifier
        {
            SqlBufferResult,
            SqlCache,
            SqlNoCache
        }

        public MySQLSelectParser(MySQLLexer lexer, MySQLExprParser exprParser)
            : base(lexer, exprParser)
        {
            this.exprParser.SelectParser = this;
        }

        public SelectStatement Select()
        {
            Match(MySQLToken.KW_SELECT);
        }

        private SelectStatement.SelectOption SelectOption()
        {
            SelectStatement.SelectOption option = new SelectStatement.SelectOption();
            MySQLToken token = MySQLToken.NONE;

            for (; ; lexer.NextToken())
            {
                token = lexer.Token();
                if (token == MySQLToken.KW_ALL)
                    option.resultDup = SelectStatement.SelectDuplicationStrategy.All;
                else if (token == MySQLToken.KW_DISTINCT)
                    option.resultDup = SelectStatement.SelectDuplicationStrategy.Distinct;
                else if (token == MySQLToken.KW_DISTINCTROW)
                    option.resultDup = SelectStatement.SelectDuplicationStrategy.DistinctRow;
                else if (token == MySQLToken.KW_HIGH_PRIORITY)
                    option.highPriority = true;
                else if (token == MySQLToken.KW_STRAIGHT_JOIN)
                    option.straightJoin = true;
                else if (token == MySQLToken.KW_SQL_SMALL_RESULT)
                    option.resultSize = SelectStatement.SmallOrBigResult.SqlSmallResult;
                else if (token == MySQLToken.KW_SQL_BIG_RESULT)
                    option.resultSize = SelectStatement.SmallOrBigResult.SqlBigResult;
                else if (token == MySQLToken.KW_SQL_CALC_FOUND_ROWS)
                    option.sqlCalcFoundRows = true;
                else if (token == MySQLToken.IDENTIFIER)
                {
                    string optionStringUp = lexer.StringValueUppercase;
                    SpecialIdentifier specialId = SpecialIdentifier.SqlBufferResult;
                    if (Enum.TryParse<SpecialIdentifier>(optionStringUp, out specialId))
                    {
                        switch (specialId)
                        {
                            case SpecialIdentifier.SqlBufferResult:
                                if (option.sqlBufferResult)
                                    return option;
                                option.sqlBufferResult = true;
                                break;
                            case SpecialIdentifier.SqlCache:
                                if (option.queryCache != SelectStatement.QueryCacheStrategy.Undefine)
                                    return option;
                                option.queryCache = SelectStatement.QueryCacheStrategy.SqlCache;
                                break;
                            case SpecialIdentifier.SqlNoCache:
                                if (option.queryCache != SelectStatement.QueryCacheStrategy.Undefine)
                                    return option;
                                option.queryCache = SelectStatement.QueryCacheStrategy.SqlNoCache;
                                break;
                        }
                    }
                }
                else
                    return option;
            }
        }

        private Dictionary<IExpression, string> SelectExprList()
        {
            IExpression expr = exprParser.
        }
    }
}
