using System;
using System.Collections.Concurrent;
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

        private static MySQLFunctionManager instance;

        public static MySQLFunctionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MySQLFunctionManager(false);
                return instance;
            }
        }

        private bool allowFuncDefChange;
        private Dictionary<string, FunctionParsingStrategy> parsingStrateg = 
            new Dictionary<string, FunctionParsingStrategy>();
        private ConcurrentDictionary<string, FunctionExpression> functionPrototype =
            new ConcurrentDictionary<string, FunctionExpression>();

        protected MySQLFunctionManager(bool allowFuncDefChange)
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

            functionPrototype.TryAdd("ABS", new AbsFunc(null));
            functionPrototype.TryAdd("ACOS", new ACosFunc(null));
            functionPrototype.TryAdd("ADDDATE", new AdddateFunc(null));
            functionPrototype.TryAdd("ADDTIME", new AddtimeFunc(null));
            functionPrototype.TryAdd("AES_DECRYPT", new AesDecryptFunc(null));
            functionPrototype.TryAdd("AES_ENCRYPT", new AesEncryptFunc(null));
            functionPrototype.TryAdd("ANALYSE", new AnalyseFunc(null));
            functionPrototype.TryAdd("ASCII", new AsciiFunc(null));
            functionPrototype.TryAdd("ASIN", new ASinFunc(null));
            functionPrototype.TryAdd("ATAN2", new ATan2Func(null));
            functionPrototype.TryAdd("ATAN", new ATanFunc(null));
            functionPrototype.TryAdd("BENCHMARK", new BenchmarkFunc(null));
            functionPrototype.TryAdd("BIN", new BinFunc(null));
            functionPrototype.TryAdd("BIT_AND", new BitAndFunc(null));
            functionPrototype.TryAdd("BIT_COUNT", new BitCountFunc(null));
            functionPrototype.TryAdd("BIT_LENGTH", new BitLengthFunc(null));
            functionPrototype.TryAdd("BIT_OR", new BitOrFunc(null));
            functionPrototype.TryAdd("BIT_XOR", new BitxOrFunc(null));
            functionPrototype.TryAdd("CEIL", new CeilingFunc(null));
            functionPrototype.TryAdd("CEILING", new CeilingFunc(null));
            functionPrototype.TryAdd("CHAR_LENGTH", new CharLengthFunc(null));
            functionPrototype.TryAdd("CHARACTER_LENGTH", new CharLengthFunc(null));
            functionPrototype.TryAdd("CHARSET", new CharsetFunc(null));
            functionPrototype.TryAdd("COALESCE", new CoalesceFunc(null));
            functionPrototype.TryAdd("COERCIBILITY", new CoercibilityFunc(null));
            functionPrototype.TryAdd("COLLATION", new CollationFunc(null));
            functionPrototype.TryAdd("COMPRESS", new CompressFunc(null));
            functionPrototype.TryAdd("CONCAT_WS", new ConcatWsFunc(null));
            functionPrototype.TryAdd("CONCAT", new ConcatFunc(null));
            functionPrototype.TryAdd("CONNECTION_ID", new ConnectionIdFunc(null));
            functionPrototype.TryAdd("CONV", new ConvFunc(null));
            functionPrototype.TryAdd("CONVERT_TZ", new ConvertTzFunc(null));
            functionPrototype.TryAdd("COS", new CosFunc(null));
            functionPrototype.TryAdd("COT", new CotFunc(null));
            functionPrototype.TryAdd("CRC32", new Crc32Func(null));
            functionPrototype.TryAdd("CURDATE", new CurdateFunc());
            functionPrototype.TryAdd("CURRENT_DATE", new CurdateFunc());
            functionPrototype.TryAdd("CURRENT_TIME", new CurtimeFunc());
            functionPrototype.TryAdd("CURTIME", new CurtimeFunc());
            functionPrototype.TryAdd("CURRENT_TIMESTAMP", new NowFunc());
            functionPrototype.TryAdd("CURRENT_USER", new CurrentUserFunc());
            functionPrototype.TryAdd("CURTIME", new CurtimeFunc());
            functionPrototype.TryAdd("DATABASE", new DatabaseFunc(null));
            functionPrototype.TryAdd("DATE_ADD", new DateAddFunc(null));
            functionPrototype.TryAdd("DATE_FORMAT", new DateFormatFunc(null));
            functionPrototype.TryAdd("DATE_SUB", new DateSubFunc(null));
            functionPrototype.TryAdd("DATE", new DateFunc(null));
            functionPrototype.TryAdd("DATEDIFF", new DateDiffFunc(null));
            functionPrototype.TryAdd("DAY", new DayofMonthFunc(null));
            functionPrototype.TryAdd("DAYOFMONTH", new DayofMonthFunc(null));
            functionPrototype.TryAdd("DAYNAME", new DaynameFunc(null));
            functionPrototype.TryAdd("DAYOFWEEK", new DayofWeekFunc(null));
            functionPrototype.TryAdd("DAYOFYEAR", new DayofYearFunc(null));
            functionPrototype.TryAdd("DECODE", new DecodeFunc(null));
            functionPrototype.TryAdd("DEFAULT", new DefaultFunc(null));
            functionPrototype.TryAdd("DEGREES", new DegreesFunc(null));
            functionPrototype.TryAdd("DES_DECRYPT", new DesDecryptFunc(null));
            functionPrototype.TryAdd("DES_ENCRYPT", new DesEncryptFunc(null));
            functionPrototype.TryAdd("ELT", new EltFunc(null));
            functionPrototype.TryAdd("ENCODE", new EncodeFunc(null));
            functionPrototype.TryAdd("ENCRYPT", new EncryptFunc(null));
            functionPrototype.TryAdd("EXP", new ExpFunc(null));
            functionPrototype.TryAdd("EXPORT_SET", new ExportSetFunc(null));
            //        functionPrototype.TryAdd("EXTRACT", new ExtractFunc(null));
            functionPrototype.TryAdd("EXTRACTVALUE", new ExtractValueFunc(null));
            functionPrototype.TryAdd("FIELD", new FieldFunc(null));
            functionPrototype.TryAdd("FIND_IN_SET", new FindInSetFunc(null));
            functionPrototype.TryAdd("FLOOR", new FloorFunc(null));
            functionPrototype.TryAdd("FORMAT", new FormatFunc(null));
            functionPrototype.TryAdd("FOUND_ROWS", new FoundRowsFunc(null));
            functionPrototype.TryAdd("FROM_DAYS", new FromDaysFunc(null));
            functionPrototype.TryAdd("FROM_UNIXTIME", new FromUnixtimeFunc(null));
            //        functionPrototype.TryAdd("GET_FORMAT", new GetFormatFunc(null));
            functionPrototype.TryAdd("GET_LOCK", new GetLockFunc(null));
            functionPrototype.TryAdd("GREATEST", new GreatestFunc(null));
            functionPrototype.TryAdd("HEX", new HexFunc(null));
            functionPrototype.TryAdd("HOUR", new HourFunc(null));
            functionPrototype.TryAdd("IF", new IfFunc(null));
            functionPrototype.TryAdd("IFNULL", new IfNullFunc(null));
            functionPrototype.TryAdd("INET_ATON", new InetAtonFunc(null));
            functionPrototype.TryAdd("INET_NTOA", new InetNtoaFunc(null));
            functionPrototype.TryAdd("INSERT", new InsertFunc(null));
            functionPrototype.TryAdd("INSTR", new InstrFunc(null));
            functionPrototype.TryAdd("INTERVAL", new IntervalFunc(null));
            functionPrototype.TryAdd("IS_FREE_LOCK", new IsFreeLockFunc(null));
            functionPrototype.TryAdd("IS_USED_LOCK", new IsUsedLockFunc(null));
            functionPrototype.TryAdd("ISNULL", new IsNullFunc(null));
            functionPrototype.TryAdd("LAST_DAY", new LastDayFunc(null));
            functionPrototype.TryAdd("LAST_INSERT_ID", new LastInsertIdFunc(null));
            functionPrototype.TryAdd("LCASE", new LowerFunc(null));
            functionPrototype.TryAdd("LEAST", new LeastFunc(null));
            functionPrototype.TryAdd("LEFT", new LeftFunc(null));
            functionPrototype.TryAdd("LENGTH", new LengthFunc(null));
            functionPrototype.TryAdd("LN", new LogFunc(null)); //Ln(X) equals Log(X)
            functionPrototype.TryAdd("LOAD_FILE", new LoadFileFunc(null));
            functionPrototype.TryAdd("LOCALTIME", new NowFunc());
            functionPrototype.TryAdd("LOCALTIMESTAMP", new NowFunc());
            functionPrototype.TryAdd("LOCATE", new LocateFunc(null));
            functionPrototype.TryAdd("LOG10", new Log10Func(null));
            functionPrototype.TryAdd("LOG2", new Log2Func(null));
            functionPrototype.TryAdd("LOG", new LogFunc(null));
            functionPrototype.TryAdd("LOWER", new LowerFunc(null));
            functionPrototype.TryAdd("LPAD", new LpadFunc(null));
            functionPrototype.TryAdd("LTRIM", new LtrimFunc(null));
            functionPrototype.TryAdd("MAKE_SET", new MakeSetFunc(null));
            functionPrototype.TryAdd("MAKEDATE", new MakedateFunc(null));
            functionPrototype.TryAdd("MAKETIME", new MaketimeFunc(null));
            functionPrototype.TryAdd("MASTER_POS_WAIT", new MasterPosWaitFunc(null));
            functionPrototype.TryAdd("MD5", new Md5Func(null));
            functionPrototype.TryAdd("MICROSECOND", new MicrosecondFunc(null));
            functionPrototype.TryAdd("MID", new SubstringFunc(null));
            functionPrototype.TryAdd("MINUTE", new MinuteFunc(null));
            functionPrototype.TryAdd("MONTH", new MonthFunc(null));
            functionPrototype.TryAdd("MONTHNAME", new MonthnameFunc(null));
            functionPrototype.TryAdd("NAME_CONST", new NameConstFunc(null));
            functionPrototype.TryAdd("NOW", new NowFunc());
            functionPrototype.TryAdd("NULLIF", new NullIfFunc(null));
            functionPrototype.TryAdd("OCT", new OctFunc(null));
            functionPrototype.TryAdd("OCTET_LENGTH", new LengthFunc(null));
            functionPrototype.TryAdd("OLD_PASSWORD", new OldPasswordFunc(null));
            functionPrototype.TryAdd("ORD", new OrdFunc(null));
            functionPrototype.TryAdd("PASSWORD", new PasswordFunc(null));
            functionPrototype.TryAdd("PERIOD_ADD", new PeriodAddFunc(null));
            functionPrototype.TryAdd("PERIOD_DIFF", new PeriodDiffFunc(null));
            functionPrototype.TryAdd("PI", new PiFunc(null));
            functionPrototype.TryAdd("POW", new PowFunc(null));
            functionPrototype.TryAdd("POWER", new PowFunc(null));
            functionPrototype.TryAdd("QUARTER", new QuarterFunc(null));
            functionPrototype.TryAdd("QUOTE", new QuoteFunc(null));
            functionPrototype.TryAdd("RADIANS", new RadiansFunc(null));
            functionPrototype.TryAdd("RAND", new RandFunc(null));
            functionPrototype.TryAdd("RELEASE_LOCK", new ReleaseLockFunc(null));
            functionPrototype.TryAdd("REPEAT", new RepeatFunc(null));
            functionPrototype.TryAdd("REPLACE", new ReplaceFunc(null));
            functionPrototype.TryAdd("REVERSE", new ReverseFunc(null));
            functionPrototype.TryAdd("RIGHT", new RightFunc(null));
            functionPrototype.TryAdd("ROUND", new RoundFunc(null));
            functionPrototype.TryAdd("ROW_COUNT", new RowCountFunc(null));
            functionPrototype.TryAdd("RPAD", new RpadFunc(null));
            functionPrototype.TryAdd("RTRIM", new RtrimFunc(null));
            functionPrototype.TryAdd("SCHEMA", new DatabaseFunc(null));
            functionPrototype.TryAdd("SEC_TO_TIME", new SecToTimeFunc(null));
            functionPrototype.TryAdd("SECOND", new SecondFunc(null));
            functionPrototype.TryAdd("SESSION_USER", new UserFunc(null));
            functionPrototype.TryAdd("SHA1", new Sha1Func(null));
            functionPrototype.TryAdd("SHA", new Sha1Func(null));
            functionPrototype.TryAdd("SHA2", new Sha2Func(null));
            functionPrototype.TryAdd("SIGN", new SignFunc(null));
            functionPrototype.TryAdd("SIN", new SinFunc(null));
            functionPrototype.TryAdd("SLEEP", new SleepFunc(null));
            functionPrototype.TryAdd("SOUNDEX", new SoundexFund(null));
            functionPrototype.TryAdd("SPACE", new SpaceFunc(null));
            functionPrototype.TryAdd("SQRT", new SqrtFunc(null));
            functionPrototype.TryAdd("STD", new StdFunc(null));
            functionPrototype.TryAdd("STDDEV_POP", new StddevPopFunc(null));
            functionPrototype.TryAdd("STDDEV_SAMP", new StddevSampFunc(null));
            functionPrototype.TryAdd("STDDEV", new StddevFunc(null));
            functionPrototype.TryAdd("STR_TO_DATE", new StrToDateFunc(null));
            functionPrototype.TryAdd("STRCMP", new StrcmpFunc(null));
            functionPrototype.TryAdd("SUBDATE", new SubdateFunc(null));
            functionPrototype.TryAdd("SUBSTRING_INDEX", new SubstringIndexFunc(null));
            functionPrototype.TryAdd("SUBTIME", new SubtimeFunc(null));
            functionPrototype.TryAdd("SYSDATE", new SysdateFunc(null));
            functionPrototype.TryAdd("SYSTEM_USER", new UserFunc(null));
            functionPrototype.TryAdd("TAN", new TanFunc(null));
            functionPrototype.TryAdd("TIME_FORMAT", new TimeFormatFunc(null));
            functionPrototype.TryAdd("TIME_TO_SEC", new TimeToSecFunc(null));
            functionPrototype.TryAdd("TIME", new TimeFunc(null));
            functionPrototype.TryAdd("TIMEDIFF", new TimediffFunc(null));
            functionPrototype.TryAdd("TIMESTAMP", new TimestampFunc(null));
            //        functionPrototype.TryAdd("TIMESTAMPADD", new TimestampaddFunc(null));
            //        functionPrototype.TryAdd("TIMESTAMPDIFF", new TimestampdiffFunc(null));
            functionPrototype.TryAdd("TO_DAYS", new ToDaysFunc(null));
            functionPrototype.TryAdd("TO_SECONDS", new ToSecondsFunc(null));
            functionPrototype.TryAdd("TRUNCATE", new TruncateFunc(null));
            functionPrototype.TryAdd("UCASE", new UpperFunc(null));
            functionPrototype.TryAdd("UNCOMPRESS", new UncompressFunc(null));
            functionPrototype.TryAdd("UNCOMPRESSED_LENGTH", new UncompressedLengthFunc(null));
            functionPrototype.TryAdd("UNHEX", new UnhexFunc(null));
            functionPrototype.TryAdd("UNIX_TIMESTAMP", new UnixTimestampFunc(null));
            functionPrototype.TryAdd("UPDATEXML", new UpdateXmlFunc(null));
            functionPrototype.TryAdd("UPPER", new UpperFunc(null));
            functionPrototype.TryAdd("USER", new UserFunc(null));
            functionPrototype.TryAdd("UTC_DATE", new UtcDateFunc(null));
            functionPrototype.TryAdd("UTC_TIME", new UtcTimeFunc(null));
            functionPrototype.TryAdd("UTC_TIMESTAMP", new UtcTimestampFunc(null));
            functionPrototype.TryAdd("UUID_SHORT", new UuidShortFunc(null));
            functionPrototype.TryAdd("UUID", new UuidFunc(null));
            functionPrototype.TryAdd("VALUES", new ValuesFunc(null));
            functionPrototype.TryAdd("VAR_POP", new VarPopFunc(null));
            functionPrototype.TryAdd("VAR_SAMP", new VarSampFunc(null));
            functionPrototype.TryAdd("VARIANCE", new VarianceFunc(null));
            functionPrototype.TryAdd("VERSION", new VersionFunc(null));
            functionPrototype.TryAdd("WEEK", new WeekFunc(null));
            functionPrototype.TryAdd("WEEKDAY", new WeekdayFunc(null));
            functionPrototype.TryAdd("WEEKOFYEAR", new WeekofyearFunc(null));
            functionPrototype.TryAdd("YEAR", new YearFunc(null));
            functionPrototype.TryAdd("YEARWEEK", new YearweekFunc(null));
        }

        public void AddExtendFunction(Dictionary<string, FunctionExpression> extFuncs)
        {
            if (extFuncs == null || extFuncs.Count == 0)
                return;
            if (!allowFuncDefChange)
                throw new NotImplementedException("function define is not allowed to be changed");

            foreach (var item in extFuncs)
            {
                string funcName = item.Key;
                if (String.IsNullOrEmpty(funcName))
                    continue;
                string funcNameup = funcName.ToUpper();
                if (functionPrototype.ContainsKey(funcNameup))
                    throw new ArgumentException("ext-function '" + funcName + "' is MySQL's predefined function!");
                FunctionExpression func = item.Value;
                if(func == null)
                    throw new ArgumentException("ext-function '" + funcName + "' is not define!");
                functionPrototype.TryAdd(funcNameup, func);
            }
        }

        public FunctionExpression CreateFunctionExpression(string funcNameUpcase, List<IExpression> arguments)
        {
            FunctionExpression prototype;
            if (functionPrototype.TryGetValue(funcNameUpcase, out prototype))
            {
                FunctionExpression func = prototype.ConstructFunction(arguments);
                func.init();
                return func;
            }
            return null;
        }

        public FunctionParsingStrategy GetParsingStrategy(string funcNameUpcase)
        {
            FunctionParsingStrategy s = parsingStrateg[funcNameUpcase];
            if (s == null)
            {
                if (functionPrototype.ContainsKey(funcNameUpcase))
                    return FunctionParsingStrategy._ORDINARY;
                return FunctionParsingStrategy._DEFAULT;
            }
            return s;
        }
    }
}
