using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    public class MySQLFunctionManager
    {
        public enum FunctionParsingStrategy
        {
            /** not a function */
            _DEFAULT,
            /** ordinary function */
            _ORDINARY,
            CAST,
            POSITION,
            SUBSTRING,
            TRIM,
            AVG,
            COUNT,
            GROUP_CONCAT,
            MAX,
            MIN,
            SUM,
            ROW,
            CHAR,
            CONVERT,
            EXTRACT,
            TIMESTAMPADD,
            TIMESTAMPDIFF,
            GET_FORMAT
        }

        private bool allowFuncDefChange;
        private Dictionary<string, FunctionParsingStrategy> parsingStrateg = 
            new Dictionary<string, FunctionParsingStrategy>();
        private Dictionary<string, FunctionExpression> functionPrototype =
            new Dictionary<string, FunctionExpression>();

        public MySQLFunctionManager(bool allowFuncDefChange)
        {
            this.allowFuncDefChange = allowFuncDefChange;
            parsingStrateg.Add("CAST", FunctionParsingStrategy.CAST);
            parsingStrateg.Add("POSITION", FunctionParsingStrategy.POSITION);
            parsingStrateg.Add("SUBSTR", FunctionParsingStrategy.SUBSTRING);
            parsingStrateg.Add("SUBSTRING", FunctionParsingStrategy.SUBSTRING);
            parsingStrateg.Add("TRIM", FunctionParsingStrategy.TRIM);
            parsingStrateg.Add("AVG", FunctionParsingStrategy.AVG);
            parsingStrateg.Add("COUNT", FunctionParsingStrategy.COUNT);
            parsingStrateg.Add("GROUP_CONCAT", FunctionParsingStrategy.GROUP_CONCAT);
            parsingStrateg.Add("MAX", FunctionParsingStrategy.MAX);
            parsingStrateg.Add("MIN", FunctionParsingStrategy.MIN);
            parsingStrateg.Add("SUM", FunctionParsingStrategy.SUM);
            parsingStrateg.Add("ROW", FunctionParsingStrategy.ROW);
            parsingStrateg.Add("CHAR", FunctionParsingStrategy.CHAR);
            parsingStrateg.Add("CONVERT", FunctionParsingStrategy.CONVERT);
            parsingStrateg.Add("EXTRACT", FunctionParsingStrategy.EXTRACT);
            parsingStrateg.Add("TIMESTAMPADD", FunctionParsingStrategy.TIMESTAMPADD);
            parsingStrateg.Add("TIMESTAMPDIFF", FunctionParsingStrategy.TIMESTAMPDIFF);
            parsingStrateg.Add("GET_FORMAT", FunctionParsingStrategy.GET_FORMAT);

            functionPrototype.Add("ABS", new Abs(null));
            functionPrototype.Add("ACOS", new Acos(null));
            functionPrototype.Add("ADDDATE", new Adddate(null));
            functionPrototype.Add("ADDTIME", new Addtime(null));
            functionPrototype.Add("AES_DECRYPT", new AesDecrypt(null));
            functionPrototype.Add("AES_ENCRYPT", new AesEncrypt(null));
            functionPrototype.Add("ANALYSE", new Analyse(null));
            functionPrototype.Add("ASCII", new Ascii(null));
            functionPrototype.Add("ASIN", new Asin(null));
            functionPrototype.Add("ATAN2", new Atan2(null));
            functionPrototype.Add("ATAN", new Atan(null));
            functionPrototype.Add("BENCHMARK", new Benchmark(null));
            functionPrototype.Add("BIN", new Bin(null));
            functionPrototype.Add("BIT_AND", new BitAnd(null));
            functionPrototype.Add("BIT_COUNT", new BitCount(null));
            functionPrototype.Add("BIT_LENGTH", new BitLength(null));
            functionPrototype.Add("BIT_OR", new BitOr(null));
            functionPrototype.Add("BIT_XOR", new BitXor(null));
            functionPrototype.Add("CEIL", new Ceiling(null));
            functionPrototype.Add("CEILING", new Ceiling(null));
            functionPrototype.Add("CHAR_LENGTH", new CharLength(null));
            functionPrototype.Add("CHARACTER_LENGTH", new CharLength(null));
            functionPrototype.Add("CHARSET", new Charset(null));
            functionPrototype.Add("COALESCE", new Coalesce(null));
            functionPrototype.Add("COERCIBILITY", new Coercibility(null));
            functionPrototype.Add("COLLATION", new Collation(null));
            functionPrototype.Add("COMPRESS", new Compress(null));
            functionPrototype.Add("CONCAT_WS", new ConcatWs(null));
            functionPrototype.Add("CONCAT", new Concat(null));
            functionPrototype.Add("CONNECTION_ID", new ConnectionId(null));
            functionPrototype.Add("CONV", new Conv(null));
            functionPrototype.Add("CONVERT_TZ", new ConvertTz(null));
            functionPrototype.Add("COS", new Cos(null));
            functionPrototype.Add("COT", new Cot(null));
            functionPrototype.Add("CRC32", new Crc32(null));
            functionPrototype.Add("CURDATE", new Curdate());
            functionPrototype.Add("CURRENT_DATE", new Curdate());
            functionPrototype.Add("CURRENT_TIME", new Curtime());
            functionPrototype.Add("CURTIME", new Curtime());
            functionPrototype.Add("CURRENT_TIMESTAMP", new Now());
            functionPrototype.Add("CURRENT_USER", new CurrentUser());
            functionPrototype.Add("CURTIME", new Curtime());
            functionPrototype.Add("DATABASE", new Database(null));
            functionPrototype.Add("DATE_ADD", new DateAdd(null));
            functionPrototype.Add("DATE_FORMAT", new DateFormat(null));
            functionPrototype.Add("DATE_SUB", new DateSub(null));
            functionPrototype.Add("DATE", new Date(null));
            functionPrototype.Add("DATEDIFF", new Datediff(null));
            functionPrototype.Add("DAY", new Dayofmonth(null));
            functionPrototype.Add("DAYOFMONTH", new Dayofmonth(null));
            functionPrototype.Add("DAYNAME", new Dayname(null));
            functionPrototype.Add("DAYOFWEEK", new Dayofweek(null));
            functionPrototype.Add("DAYOFYEAR", new Dayofyear(null));
            functionPrototype.Add("DECODE", new Decode(null));
            functionPrototype.Add("DEFAULT", new Default(null));
            functionPrototype.Add("DEGREES", new Degrees(null));
            functionPrototype.Add("DES_DECRYPT", new DesDecrypt(null));
            functionPrototype.Add("DES_ENCRYPT", new DesEncrypt(null));
            functionPrototype.Add("ELT", new Elt(null));
            functionPrototype.Add("ENCODE", new Encode(null));
            functionPrototype.Add("ENCRYPT", new Encrypt(null));
            functionPrototype.Add("EXP", new Exp(null));
            functionPrototype.Add("EXPORT_SET", new ExportSet(null));
            //        functionPrototype.Add("EXTRACT", new Extract(null));
            functionPrototype.Add("EXTRACTVALUE", new Extractvalue(null));
            functionPrototype.Add("FIELD", new Field(null));
            functionPrototype.Add("FIND_IN_SET", new FindInSet(null));
            functionPrototype.Add("FLOOR", new Floor(null));
            functionPrototype.Add("FORMAT", new Format(null));
            functionPrototype.Add("FOUND_ROWS", new FoundRows(null));
            functionPrototype.Add("FROM_DAYS", new FromDays(null));
            functionPrototype.Add("FROM_UNIXTIME", new FromUnixtime(null));
            //        functionPrototype.Add("GET_FORMAT", new GetFormat(null));
            functionPrototype.Add("GET_LOCK", new GetLock(null));
            functionPrototype.Add("GREATEST", new Greatest(null));
            functionPrototype.Add("HEX", new Hex(null));
            functionPrototype.Add("HOUR", new Hour(null));
            functionPrototype.Add("IF", new If(null));
            functionPrototype.Add("IFNULL", new Ifnull(null));
            functionPrototype.Add("INET_ATON", new InetAton(null));
            functionPrototype.Add("INET_NTOA", new InetNtoa(null));
            functionPrototype.Add("INSERT", new Insert(null));
            functionPrototype.Add("INSTR", new Instr(null));
            functionPrototype.Add("INTERVAL", new Interval(null));
            functionPrototype.Add("IS_FREE_LOCK", new IsFreeLock(null));
            functionPrototype.Add("IS_USED_LOCK", new IsUsedLock(null));
            functionPrototype.Add("ISNULL", new Isnull(null));
            functionPrototype.Add("LAST_DAY", new LastDay(null));
            functionPrototype.Add("LAST_INSERT_ID", new LastInsertId(null));
            functionPrototype.Add("LCASE", new Lower(null));
            functionPrototype.Add("LEAST", new Least(null));
            functionPrototype.Add("LEFT", new Left(null));
            functionPrototype.Add("LENGTH", new Length(null));
            functionPrototype.Add("LN", new Log(null)); //Ln(X) equals Log(X)
            functionPrototype.Add("LOAD_FILE", new LoadFile(null));
            functionPrototype.Add("LOCALTIME", new Now());
            functionPrototype.Add("LOCALTIMESTAMP", new Now());
            functionPrototype.Add("LOCATE", new Locate(null));
            functionPrototype.Add("LOG10", new Log10(null));
            functionPrototype.Add("LOG2", new Log2(null));
            functionPrototype.Add("LOG", new Log(null));
            functionPrototype.Add("LOWER", new Lower(null));
            functionPrototype.Add("LPAD", new Lpad(null));
            functionPrototype.Add("LTRIM", new Ltrim(null));
            functionPrototype.Add("MAKE_SET", new MakeSet(null));
            functionPrototype.Add("MAKEDATE", new Makedate(null));
            functionPrototype.Add("MAKETIME", new Maketime(null));
            functionPrototype.Add("MASTER_POS_WAIT", new MasterPosWait(null));
            functionPrototype.Add("MD5", new Md5(null));
            functionPrototype.Add("MICROSECOND", new Microsecond(null));
            functionPrototype.Add("MID", new Substring(null));
            functionPrototype.Add("MINUTE", new Minute(null));
            functionPrototype.Add("MONTH", new Month(null));
            functionPrototype.Add("MONTHNAME", new Monthname(null));
            functionPrototype.Add("NAME_CONST", new NameConst(null));
            functionPrototype.Add("NOW", new Now());
            functionPrototype.Add("NULLIF", new Nullif(null));
            functionPrototype.Add("OCT", new Oct(null));
            functionPrototype.Add("OCTET_LENGTH", new Length(null));
            functionPrototype.Add("OLD_PASSWORD", new OldPassword(null));
            functionPrototype.Add("ORD", new Ord(null));
            functionPrototype.Add("PASSWORD", new Password(null));
            functionPrototype.Add("PERIOD_ADD", new PeriodAdd(null));
            functionPrototype.Add("PERIOD_DIFF", new PeriodDiff(null));
            functionPrototype.Add("PI", new Pi(null));
            functionPrototype.Add("POW", new Pow(null));
            functionPrototype.Add("POWER", new Pow(null));
            functionPrototype.Add("QUARTER", new Quarter(null));
            functionPrototype.Add("QUOTE", new Quote(null));
            functionPrototype.Add("RADIANS", new Radians(null));
            functionPrototype.Add("RAND", new Rand(null));
            functionPrototype.Add("RELEASE_LOCK", new ReleaseLock(null));
            functionPrototype.Add("REPEAT", new Repeat(null));
            functionPrototype.Add("REPLACE", new Replace(null));
            functionPrototype.Add("REVERSE", new Reverse(null));
            functionPrototype.Add("RIGHT", new Right(null));
            functionPrototype.Add("ROUND", new Round(null));
            functionPrototype.Add("ROW_COUNT", new RowCount(null));
            functionPrototype.Add("RPAD", new Rpad(null));
            functionPrototype.Add("RTRIM", new Rtrim(null));
            functionPrototype.Add("SCHEMA", new Database(null));
            functionPrototype.Add("SEC_TO_TIME", new SecToTime(null));
            functionPrototype.Add("SECOND", new Second(null));
            functionPrototype.Add("SESSION_USER", new User(null));
            functionPrototype.Add("SHA1", new Sha1(null));
            functionPrototype.Add("SHA", new Sha1(null));
            functionPrototype.Add("SHA2", new Sha2(null));
            functionPrototype.Add("SIGN", new SignFunc(null));
            functionPrototype.Add("SIN", new Sin(null));
            functionPrototype.Add("SLEEP", new Sleep(null));
            functionPrototype.Add("SOUNDEX", new Soundex(null));
            functionPrototype.Add("SPACE", new Space(null));
            functionPrototype.Add("SQRT", new Sqrt(null));
            functionPrototype.Add("STD", new Std(null));
            functionPrototype.Add("STDDEV_POP", new StddevPop(null));
            functionPrototype.Add("STDDEV_SAMP", new StddevSamp(null));
            functionPrototype.Add("STDDEV", new Stddev(null));
            functionPrototype.Add("STR_TO_DATE", new StrToDate(null));
            functionPrototype.Add("STRCMP", new Strcmp(null));
            functionPrototype.Add("SUBDATE", new Subdate(null));
            functionPrototype.Add("SUBSTRING_INDEX", new SubstringIndex(null));
            functionPrototype.Add("SUBTIME", new Subtime(null));
            functionPrototype.Add("SYSDATE", new Sysdate(null));
            functionPrototype.Add("SYSTEM_USER", new User(null));
            functionPrototype.Add("TAN", new Tan(null));
            functionPrototype.Add("TIME_FORMAT", new TimeFormat(null));
            functionPrototype.Add("TIME_TO_SEC", new TimeToSec(null));
            functionPrototype.Add("TIME", new Time(null));
            functionPrototype.Add("TIMEDIFF", new Timediff(null));
            functionPrototype.Add("TIMESTAMP", new Timestamp(null));
            //        functionPrototype.Add("TIMESTAMPADD", new Timestampadd(null));
            //        functionPrototype.Add("TIMESTAMPDIFF", new Timestampdiff(null));
            functionPrototype.Add("TO_DAYS", new ToDays(null));
            functionPrototype.Add("TO_SECONDS", new ToSeconds(null));
            functionPrototype.Add("TRUNCATE", new Truncate(null));
            functionPrototype.Add("UCASE", new Upper(null));
            functionPrototype.Add("UNCOMPRESS", new Uncompress(null));
            functionPrototype.Add("UNCOMPRESSED_LENGTH", new UncompressedLength(null));
            functionPrototype.Add("UNHEX", new Unhex(null));
            functionPrototype.Add("UNIX_TIMESTAMP", new UnixTimestamp(null));
            functionPrototype.Add("UPDATEXML", new Updatexml(null));
            functionPrototype.Add("UPPER", new Upper(null));
            functionPrototype.Add("USER", new User(null));
            functionPrototype.Add("UTC_DATE", new UtcDate(null));
            functionPrototype.Add("UTC_TIME", new UtcTime(null));
            functionPrototype.Add("UTC_TIMESTAMP", new UtcTimestamp(null));
            functionPrototype.Add("UUID_SHORT", new UuidShort(null));
            functionPrototype.Add("UUID", new Uuid(null));
            functionPrototype.Add("VALUES", new Values(null));
            functionPrototype.Add("VAR_POP", new VarPop(null));
            functionPrototype.Add("VAR_SAMP", new VarSamp(null));
            functionPrototype.Add("VARIANCE", new Variance(null));
            functionPrototype.Add("VERSION", new Version(null));
            functionPrototype.Add("WEEK", new Week(null));
            functionPrototype.Add("WEEKDAY", new Weekday(null));
            functionPrototype.Add("WEEKOFYEAR", new Weekofyear(null));
            functionPrototype.Add("YEAR", new Year(null));
            functionPrototype.Add("YEARWEEK", new Yearweek(null));
        }
    }
}
