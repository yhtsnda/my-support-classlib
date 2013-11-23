using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public class NullLogFactory : ILogFactory
    {
        public ILog GetLogger(string name)
        {
            return NullLogger.Instance;
        }

        public ILog GetLogger(Type type)
        {
            return NullLogger.Instance;
        }

        private class NullLogger : ILog
        {
            internal static readonly ILog Instance = new NullLogFactory.NullLogger();

            private NullLogger() { }

            void ILog.Debug(object message) { }
            void ILog.Debug(object message, Exception exception) { }
            void ILog.DebugFormat(string format, object arg0) { }
            void ILog.DebugFormat(string format, params object[] args) { }
            void ILog.DebugFormat(IFormatProvider provider, string format, params object[] args) { }
            void ILog.DebugFormat(string format, object arg0, object arg1) { }
            void ILog.DebugFormat(string format, object arg0, object arg1, object arg2) { }

            void ILog.Error(object message) { }
            void ILog.Error(object message, Exception exception) { }
            void ILog.ErrorFormat(string format, params object[] args) { }
            void ILog.ErrorFormat(string format, object arg0) { }
            void ILog.ErrorFormat(IFormatProvider provider, string format, params object[] args) { }
            void ILog.ErrorFormat(string format, object arg0, object arg1) { }
            void ILog.ErrorFormat(string format, object arg0, object arg1, object arg2) { }

            void ILog.Fatal(object message) { }
            void ILog.Fatal(object message, Exception exception) { }
            void ILog.FatalFormat(string format, params object[] args) { }
            void ILog.FatalFormat(string format, object arg0) { }
            void ILog.FatalFormat(IFormatProvider provider, string format, params object[] args) { }
            void ILog.FatalFormat(string format, object arg0, object arg1) { }
            void ILog.FatalFormat(string format, object arg0, object arg1, object arg2) { }

            void ILog.Info(object message) { }
            void ILog.Info(object message, Exception exception) { }
            void ILog.InfoFormat(string format, params object[] args) { }
            void ILog.InfoFormat(string format, object arg0) { }
            void ILog.InfoFormat(IFormatProvider provider, string format, params object[] args) { }
            void ILog.InfoFormat(string format, object arg0, object arg1) { }
            void ILog.InfoFormat(string format, object arg0, object arg1, object arg2) { }

            void ILog.Warn(object message) { }
            void ILog.Warn(object message, Exception exception) { }
            void ILog.WarnFormat(string format, params object[] args) { }
            void ILog.WarnFormat(string format, object arg0) { }
            void ILog.WarnFormat(IFormatProvider provider, string format, params object[] args) { }
            void ILog.WarnFormat(string format, object arg0, object arg1) { }
            void ILog.WarnFormat(string format, object arg0, object arg1, object arg2) { }

            bool ILog.IsDebugEnabled { get { return false; } }
            bool ILog.IsErrorEnabled { get { return false; } }
            bool ILog.IsFatalEnabled { get { return false; } }
            bool ILog.IsInfoEnabled { get { return false; } }
            bool ILog.IsWarnEnabled { get { return false; } }
        }

        public void Flush()
        {
            
        }
    }
}
