using Projects.Tool;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Projects.Framework.NHibernateAccess
{
    /// <summary>
    /// ProfilterLoggerFactory
    /// </summary>
    public class ProfilterLoggerFactory : ILoggerFactory
    {
        static readonly string NHibernateSQL = "NHibernate.SQL";

        static IInternalLogger defaultLogger = new NoLoggingInternalLogger();
        static ProfilterLogger profilterLogger = new ProfilterLogger();

        /// <summary>
        /// LoggerFor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IInternalLogger LoggerFor(Type type)
        {
            return defaultLogger;
        }

        /// <summary>
        /// LoggerFor
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public IInternalLogger LoggerFor(string keyName)
        {
            if (keyName == NHibernateSQL)
                return profilterLogger;

            return defaultLogger;
        }

        /// <summary>
        /// ProfilterLogger
        /// </summary>
        public class ProfilterLogger : IInternalLogger
        {
            //private static ILog _log = LogManager.GetLogger(NHibernateSQL);

            /// <summary>
            /// IsDebugEnabled
            /// </summary>
            public bool IsDebugEnabled
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// IsErrorEnabled
            /// </summary>
            public bool IsErrorEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// IsFatalEnabled
            /// </summary>
            public bool IsFatalEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// IsInfoEnabled
            /// </summary>
            public bool IsInfoEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// IsWarnEnabled
            /// </summary>
            public bool IsWarnEnabled
            {
                get { return true; }
            }

            /// <summary>
            /// Debug
            /// </summary>
            /// <param name="message"></param>
            /// <param name="exception"></param>
            public void Debug(object message, Exception exception)
            {
                //_log.Debug(message, exception);
            }

            /// <summary>
            /// Debug
            /// </summary>
            /// <param name="message"></param>
            public void Debug(object message)
            {
                if (ProfilerContext.Current.Enabled)
                {
                    ProfilerContext.Current.Trace("nhibernate", FormatSql((string)message));
                }
                using (var p = ProfilerContext.Profile((string)message))
                { }

                //_log.Debug(message);
            }

            static Regex fromRegex = new Regex(@"\sFROM\s(.*?)\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string FormatSql(string message)
            {
                var m = fromRegex.Match(message);
                if (m.Success)
                    return "[" + m.Groups[1].Value + "] \r\n" + message;
                return message;
            }

            /// <summary>
            /// DebugFormat
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public void DebugFormat(string format, params object[] args)
            {
                //_log.DebugFormat(format, args);
            }

            /// <summary>
            /// Error
            /// </summary>
            /// <param name="message"></param>
            /// <param name="exception"></param>
            public void Error(object message, Exception exception)
            {
                //_log.Error(message, exception);
            }

            /// <summary>
            /// Error
            /// </summary>
            /// <param name="message"></param>
            public void Error(object message)
            {
                //_log.Error(message);
            }

            /// <summary>
            /// ErrorFormat
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public void ErrorFormat(string format, params object[] args)
            {
                //_log.ErrorFormat(format, args);
            }

            /// <summary>
            /// Fatal
            /// </summary>
            /// <param name="message"></param>
            /// <param name="exception"></param>
            public void Fatal(object message, Exception exception)
            {
                //_log.Fatal(message, exception);
            }

            /// <summary>
            /// Fatal
            /// </summary>
            /// <param name="message"></param>
            public void Fatal(object message)
            {
                //_log.Fatal(message);
            }

            /// <summary>
            /// Info
            /// </summary>
            /// <param name="message"></param>
            /// <param name="exception"></param>
            public void Info(object message, Exception exception)
            {
                //_log.Info(message, exception);
            }

            /// <summary>
            /// Info
            /// </summary>
            /// <param name="message"></param>
            public void Info(object message)
            {
                //_log.Info(message);
            }

            /// <summary>
            /// InfoFormat
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public void InfoFormat(string format, params object[] args)
            {
                //_log.InfoFormat(format, args);
            }

            /// <summary>
            /// Warn
            /// </summary>
            /// <param name="message"></param>
            /// <param name="exception"></param>
            public void Warn(object message, Exception exception)
            {
                //_log.Warn(message, exception);
            }

            /// <summary>
            /// Warn
            /// </summary>
            /// <param name="message"></param>
            public void Warn(object message)
            {
                //_log.Warn(message);
            }

            /// <summary>
            /// WarnFormat
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            public void WarnFormat(string format, params object[] args)
            {
                //_log.WarnFormat(format, args);
            }
        }
    }
}
