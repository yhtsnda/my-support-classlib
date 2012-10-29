using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logger = log4net;

using Projects.Tool.Interfaces;

namespace Projects.Tool.Log4NetProvider
{
    public class Log4NetLog : ILog
    {
        Logger.ILog mLog;

        public Log4NetLog(Logger.ILog mLog)
        {
            this.mLog = mLog;
        }

        public bool IsDebugEnabled { get { return mLog.IsDebugEnabled; } }
        public bool IsErrorEnabled { get { return mLog.IsErrorEnabled; } }
        public bool IsFatalEnabled { get { return mLog.IsFatalEnabled; } }
        public bool IsInfoEnabled { get { return mLog.IsInfoEnabled; } }
        public bool IsWarnEnabled { get { return mLog.IsWarnEnabled; } }

        #region Debug
        public void Debug(object message)
        {
            mLog.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            mLog.Debug(message, exception);
        }

        public void DebugFormat(string format, object arg0)
        {
            mLog.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            mLog.DebugFormat(format, args);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            mLog.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            mLog.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            mLog.DebugFormat(format, arg0, arg1, arg2);
        }
        #endregion

        #region Error
        public void Error(object message)
        {
            mLog.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            mLog.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg0)
        {
            mLog.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            mLog.ErrorFormat(format, args);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            mLog.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            mLog.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            mLog.ErrorFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region Fatal
        public void Fatal(object message)
        {
            mLog.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            mLog.Fatal(message, exception);
        }

        public void FatalFormat(string format, object arg0)
        {
            mLog.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            mLog.FatalFormat(format, args);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            mLog.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            mLog.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            mLog.FatalFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region Info
        public void Info(object message)
        {
            mLog.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            mLog.Info(message, exception);
        }

        public void InfoFormat(string format, object arg0)
        {
            mLog.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, params object[] args)
        {
            mLog.InfoFormat(format, args);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            mLog.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            mLog.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            mLog.InfoFormat(format, arg0, arg1, arg2);
        }

        #endregion

        #region Warn
        public void Warn(object message)
        {
            mLog.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            mLog.Warn(message, exception);
        }

        public void WarnFormat(string format, object arg0)
        {
            mLog.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, params object[] args)
        {
            mLog.WarnFormat(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            mLog.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            mLog.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            mLog.WarnFormat(format, arg0, arg1, arg2);
        }

        #endregion
    }
}
