using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace Projects.Tool.WeblogProvider
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    internal class LogManager
    {
        #region 私有字段

        /// <summary>
        /// 每次批量提交的数量
        /// </summary>
        private static readonly int recordCount = 10;

        /// <summary>
        /// 读写锁
        /// </summary>
        private static readonly ReaderWriterLockSlim readWriteLocker = new ReaderWriterLockSlim();

        /// <summary>
        /// 序列化器
        /// </summary>
        private static readonly JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// 当前定义的日志记录级别
        /// </summary>
        private static LogLevel currentlogLevel = LogLevel.WARN;

        /// <summary>
        /// 接口地址
        /// </summary>
        private static string apiUrl = string.Empty;

        /// <summary>
        /// 应用Id
        /// </summary>
        private static int appId;

        /// <summary>
        /// 定时器执行时间间隔
        /// </summary>
        private static int interval = 60;

        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer timer = null;

        /// <summary>
        /// 定时任务是否执行中
        /// </summary>
        private static bool running = false;

        /// <summary>
        /// 日志存储容器
        /// </summary>
        private static List<LogEntity> logList = new List<LogEntity>(1000);

        #endregion

        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LogManager()
        {
            string connName = ToolSection.Instance.TryGetValue("log/logProvider/connectionName");
            if (connName.IsNullOrEmpty())
                throw new ConfigurationErrorsException("需要配置项目 log/logProvider/connectionName");

            if (ConfigurationManager.ConnectionStrings[connName] == null)
                throw new ConfigurationErrorsException(string.Format("缺少命名为“{0}”的连接字符串", connName));

            string connString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;

            ParseConnectionString(connString);

            InitializeTimer();
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private LogManager()
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 当前定义的日志记录级别
        /// </summary>
        public static LogLevel CurrentlogLevel
        {
            get { return currentlogLevel; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="logger">日志记录着</param>
        /// <param name="message">消息</param>
        /// <param name="exception">异常对象</param>
        /// <param name="logLevel">日志级别</param>
        public static void Append(string logger, string message, Exception exception, LogLevel logLevel)
        {
            //获取当前上下文
            HttpContext context = HttpContext.Current;
            if (logger.IsNullOrEmpty() || logLevel < currentlogLevel || (message == null && exception == null))
                return;

            var log = new LogEntity
            {
                AppId = appId,
                CreateTime = DateTime.Now,
                Exception = exception != null ? exception.ToString() : string.Empty,
                Level = logLevel,
                Logger = logger,
                Message = message ?? string.Empty,
                StackTrace = LogUtil.GetStackTrace()
            };
            readWriteLocker.EnterWriteLock();
            try
            {
                logList.Add(log);
            }
            finally
            {
                readWriteLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public static void Flush()
        {
            SaveLogs(null);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化计时器
        /// </summary>
        private static void InitializeTimer()
        {
            timer = new Timer(new TimerCallback(SaveLogs), null, interval * 1000, interval * 1000);
            serializer.RegisterConverters(new JavaScriptConverter[]
                                              {
                                                  new LogEntityConverter()
                                              });
        }

        /// <summary>
        /// 解析连接字符串
        /// </summary>
        /// <param name="connString">连接字符串</param>
        private static void ParseConnectionString(string connString)
        {
            if (String.IsNullOrEmpty(connString))
                throw new ArgumentNullException("日志服务器的连接串不能为空");

            string[] items = connString.Split(';');
            foreach (var kv in items)
            {
                if (kv.IsNullOrWhiteSpace())
                    continue;

                var vs = kv.Split('=');
                if (vs.Length != 2)
                    throw new ArgumentException("日志服务器的连接串在 " + kv + " 处存在错误");
                var key = vs[0].Trim().ToLower();
                var value = vs[1].Trim();
                switch (key)
                {
                    case "server":
                        if (value.ToLower().StartsWith("http"))
                            apiUrl = value;
                        else
                            apiUrl = string.Concat("http://", value);
                        if (apiUrl.EndsWith("/"))
                            apiUrl = apiUrl + "api/log/savebatch";
                        else
                            apiUrl = apiUrl + "/api/log/savebatch";
                        break;
                    case "level":
                        int logLevel = 3;
                        int.TryParse(value, out logLevel);
                        if (logLevel > 0)
                            currentlogLevel = (LogLevel)logLevel;
                        break;
                    case "appid":
                        int varAppId = 0;
                        int.TryParse(value, out varAppId);
                        appId = varAppId;
                        break;
                    case "interval":
                        int varInterval = 60;
                        int.TryParse(value, out varInterval);
                        interval = varInterval;
                        break;
                    default:
                        throw new ArgumentException("无法识别的日志服务器连接串属性，发生在 " + kv);
                }
            }
        }

        /// <summary>
        /// 保存日志队列
        /// </summary>
        /// <param name="sender"></param>
        private static void SaveLogs(object sender)
        {
            if (running)
                return;

            List<LogEntity> postList;
            readWriteLocker.EnterWriteLock();
            try
            {
                if (running)
                    return;

                running = true;

                postList = logList.ToList();
                logList = new List<LogEntity>(1000);
            }
            finally
            {
                readWriteLocker.ExitWriteLock();
            }

            if (postList.Count > 0)
            {
                //存储到日志中心库
                int repeatCount = (int)Math.Ceiling(postList.Count * 1.0D / recordCount);
                for (int i = 0; i < repeatCount; i++)
                {
                    var data = postList.Skip(i * recordCount).Take(recordCount).ToList();
                    PostData(data);
                }
                ////发送邮件
                //SendEmail(postList);
            }

            readWriteLocker.EnterWriteLock();
            try
            {
                running = false;
            }
            finally
            {
                readWriteLocker.ExitWriteLock();
            }
        }

        ///// <summary>
        ///// 发送邮件提醒
        ///// </summary>
        ///// <param name="list">日志列表</param>
        //private static void SendEmail(IEnumerable<LogEntity> list)
        //{
        //    //获取当前应用的ID
        //    var applicationId = list.FirstOrDefault().AppId;
        //    //获取某个应用符合条件的日志
        //    var listCondition = list.Where(o => o.AppId == applicationId && o.Level == LogLevel.FATAL).Select(o => new
        //                                                       {
        //                                                           o.Logger, o.Message, o.Exception, Url = o.StackTrace.AbsolutePath
        //                                                       }).Distinct().ToList();

        //    //没有符合发送邮件条件的日志
        //    if (listCondition.Count < 1)
        //    {
        //        return;
        //    }

        //    //获取此应用的信息
        //    string appUrl = apiUrl.Substring(0, apiUrl.LastIndexOf(@"/") + 1) + "GetApp";
        //    string json = GetPostResponse(appUrl, string.Concat("appId=", applicationId));
        //    if (string.IsNullOrEmpty(json))
        //    {
        //        return;
        //    }

        //    AppEntity app = new JavaScriptSerializer().Deserialize<AppEntity>(json);
        //    foreach (var logEntity in listCondition)
        //    {
        //        //是否提醒
        //        if (app.IsNotified)
        //        {
        //            string body = string.Format("应用：{0}<br />模块：{1}<br />消息：{2}<br />异常：{3}<br />来源：{4}", app.Title, logEntity.Logger, logEntity.Message, logEntity.Exception, logEntity.Url);
        //            Email.SendMail(app.Emails.Split(',').ToList(), body);
        //        }
        //    }
        //}

        /// <summary>
        /// 发送POST请求保存数据
        /// </summary>
        /// <param name="logs">日志对象实体列表</param>
        private static void PostData(List<LogEntity> logs)
        {
            try
            {
                LogUtil.GetPostResponse(apiUrl, serializer.Serialize(logs));
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
