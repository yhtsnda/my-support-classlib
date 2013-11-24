using Avalon.Profiler;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Avalon.NHibernateAccess
{
    /// <summary>
    /// ProfilterLoggerFactory
    /// </summary>
    public class ProfilterLoggerFactory : ILoggerFactory
    {
        static readonly string NHibernateSQL = "NHibernate.SQL";
        static ILoggerFactory DefaultLoggerFactory;

        static Regex fromRegex = new Regex(@"\sFROM\s(.*?)\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static ProfilterLoggerFactory()
        {
            var type = typeof(LoggerProvider);
            LoggerProvider loggerProvider = (LoggerProvider)type.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

            DefaultLoggerFactory = (ILoggerFactory)type.GetField("loggerFactory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(loggerProvider);
        }

        /// <summary>
        /// LoggerFor
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IInternalLogger LoggerFor(Type type)
        {
            return DefaultLoggerFactory.LoggerFor(type);
        }

        /// <summary>
        /// LoggerFor
        /// </summary>
        public IInternalLogger LoggerFor(string keyName)
        {
            var logger = DefaultLoggerFactory.LoggerFor(keyName);
            if (keyName == NHibernateSQL)
                return new ProfilterLogger(logger);

            return logger;
        }

        /// <summary>
        /// ProfilterLogger
        /// </summary>
        public class ProfilterLogger : IInternalLogger
        {
            IInternalLogger innerLogger;

            internal ProfilterLogger(IInternalLogger innerLogger)
            {
                this.innerLogger = innerLogger;
            }

            /// <summary>
            /// IsDebugEnabled
            /// </summary>
            public bool IsDebugEnabled
            {
                get { return true; }
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
            public void Debug(object message, Exception exception)
            {
                innerLogger.Debug(message, exception);
            }

            /// <summary>
            /// Debug
            /// </summary>
            public void Debug(object message)
            {
                innerLogger.Debug(message);
                if (ProfilerContext.Current.Enabled)
                {
                    string sql = (string)message;
                    ProfilerContext.Current.Trace("nhibernate", FormatSql((string)message));
                }
                using (var p = ProfilerContext.Watch((string)message))
                { }
            }


            /// <summary>
            /// DebugFormat
            /// </summary>
            public void DebugFormat(string format, params object[] args)
            {
                innerLogger.DebugFormat(format, args);
            }

            /// <summary>
            /// Error
            /// </summary>
            public void Error(object message, Exception exception)
            {
                innerLogger.Error(message, exception);
            }

            /// <summary>
            /// Error
            /// </summary>
            public void Error(object message)
            {
                innerLogger.Error(message);
            }

            /// <summary>
            /// ErrorFormat
            /// </summary>
            public void ErrorFormat(string format, params object[] args)
            {
                innerLogger.ErrorFormat(format, args);
            }

            /// <summary>
            /// Fatal
            /// </summary>
            public void Fatal(object message, Exception exception)
            {
                innerLogger.Fatal(message, exception);
            }

            /// <summary>
            /// Fatal
            /// </summary>
            public void Fatal(object message)
            {
                innerLogger.Fatal(message);
            }

            /// <summary>
            /// Info
            /// </summary>
            public void Info(object message, Exception exception)
            {
                innerLogger.Info(message, exception);
            }

            /// <summary>
            /// Info
            /// </summary>
            public void Info(object message)
            {
                innerLogger.Info(message);
            }

            /// <summary>
            /// InfoFormat
            /// </summary>
            public void InfoFormat(string format, params object[] args)
            {
                innerLogger.InfoFormat(format, args);
            }

            /// <summary>
            /// Warn
            /// </summary>
            public void Warn(object message, Exception exception)
            {
                innerLogger.Warn(message, exception);
            }

            /// <summary>
            /// Warn
            /// </summary>
            /// <param name="message"></param>
            public void Warn(object message)
            {
                innerLogger.Warn(message);
            }

            /// <summary>
            /// WarnFormat
            /// </summary>
            public void WarnFormat(string format, params object[] args)
            {
                innerLogger.WarnFormat(format, args);
            }


            string FormatSql(string message)
            {
                var m = fromRegex.Match(message);
                if (m.Success)
                    return "[" + m.Groups[1].Value + "] \r\n" + message;
                return message;
            }
        }
    }
}
