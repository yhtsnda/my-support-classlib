using System;
using Projects.Tool.Interfaces;

namespace Projects.Tool.WeblogProvider
{
    public class Log : ILog
    {
        /// <summary>
        /// 日志记录者
        /// </summary>
        private readonly string logger;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">日志记录者</param>
        public Log(string name)
        {
            logger = name;
        }

        #region 属性

        /// <summary>
        /// 是否可以记录DEBUG及DEBUG级别以上的错误信息
        /// </summary>
        public bool IsDebugEnabled
        {
            get { return LogManager.CurrentlogLevel >= LogLevel.DEBUG; }
        }

        /// <summary>
        /// 是否可以记录INFO及INFO级别以上的错误信息
        /// </summary>
        public bool IsInfoEnabled
        {
            get { return LogManager.CurrentlogLevel >= LogLevel.INFO; }
        }

        /// <summary>
        /// 是否可以记录WARN及WARN级别以上的错误信息
        /// </summary>
        public bool IsWarnEnabled
        {
            get { return LogManager.CurrentlogLevel >= LogLevel.WARN; }
        }

        /// <summary>
        /// 是否可以记录ERROR及ERROR级别以上的错误信息
        /// </summary>
        public bool IsErrorEnabled
        {
            get { return LogManager.CurrentlogLevel >= LogLevel.ERROR; }
        }

        /// <summary>
        /// 是否可以记录FATAL及FATAL级别以上的错误信息
        /// </summary>
        public bool IsFatalEnabled
        {
            get { return LogManager.CurrentlogLevel >= LogLevel.FATAL; }
        }

        #endregion

        #region 公共方法
        #region Debug
        /// <summary>
        /// 记录Debug级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void Debug(object message)
        {
            Debug(message, null);
        }

        /// <summary>
        ///  记录Debug级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="exception">异常对象</param>
        public void Debug(object message, Exception exception)
        {
            Debug(message.ToString(), exception);
        }

        /// <summary>
        /// 以指定格式记录Debug级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">格式化参数</param>
        public void DebugFormat(string format, object arg0)
        {
            Debug(string.Format(format, arg0));
        }

        /// <summary>
        /// 以指定格式记录Debug级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void DebugFormat(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        /// <summary>
        /// 以指定格式记录Debug级别的错误信息
        /// </summary>
        /// <param name="provider">格式化提供对象</param>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Debug(string.Format(provider, format, args));
        }

        /// <summary>
        /// 以指定格式记录Debug级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            Debug(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 以指定格式记录Debug级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Debug(string.Format(format, arg0, arg1, arg2));
        }
        #endregion

        #region Info
        /// <summary>
        /// 记录Info级别的错误信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            Info(message, null);
        }

        /// <summary>
        /// 记录Info级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="exception">异常对象</param>
        public void Info(object message, Exception exception)
        {
            Info(message.ToString(), exception);
        }

        /// <summary>
        /// 以指定格式记录Info级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">格式化参数</param>
        public void InfoFormat(string format, object arg0)
        {
            Info(string.Format(format, arg0));
        }

        /// <summary>
        /// 以指定格式记录Info级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void InfoFormat(string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        /// <summary>
        /// 以指定格式记录Info级别的错误信息
        /// </summary>
        /// <param name="provider">格式化提供对象</param>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Info(string.Format(provider, format, args));
        }

        /// <summary>
        /// 以指定格式记录Info级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            Info(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 以指定格式记录Info级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Info(string.Format(format, arg0, arg1, arg2));
        }
        #endregion

        #region Warn
        /// <summary>
        /// 记录Warn级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void Warn(object message)
        {
            Warn(message, null);
        }

        /// <summary>
        /// 记录Warn级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="exception">异常信息</param>
        public void Warn(object message, Exception exception)
        {
            Warn(message.ToString(), exception);
        }

        /// <summary>
        /// 以指定格式记录Warn级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">格式化参数</param>
        public void WarnFormat(string format, object arg0)
        {
            Warn(string.Format(format, arg0));
        }

        /// <summary>
        /// 以指定格式记录Warn级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void WarnFormat(string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        /// <summary>
        /// 以指定格式记录Warn级别的错误信息
        /// </summary>
        /// <param name="provider">格式化提供对象</param>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Warn(string.Format(provider, format, args));
        }

        /// <summary>
        /// 以指定格式记录Warn级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            Warn(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 以指定格式记录Warn级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Warn(string.Format(format, arg0, arg1, arg2));
        }
        #endregion

        #region Error
        /// <summary>
        /// 记录Error级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void Error(object message)
        {
            Error(message, null);
        }

        /// <summary>
        /// 记录Error级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="exception">异常对象</param>
        public void Error(object message, Exception exception)
        {
            Error(message.ToString(), exception);
        }

        /// <summary>
        /// 以指定格式记录Error级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">格式化参数</param>
        public void ErrorFormat(string format, object arg0)
        {
            Error(string.Format(format, arg0));
        }

        /// <summary>
        /// 以指定格式记录Error级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void ErrorFormat(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        /// <summary>
        /// 以指定格式记录Error级别的错误信息
        /// </summary>
        /// <param name="provider">格式化提供对象</param>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Error(string.Format(provider, format, args));
        }

        /// <summary>
        /// 以指定格式记录Error级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            Error(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 以指定格式记录Error级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Error(string.Format(format, arg0, arg1, arg2));
        }
        #endregion

        #region Fatal
        /// <summary>
        /// 记录Fatal级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        public void Fatal(object message)
        {
            Fatal(message, null);
        }

        /// <summary>
        /// 记录Fatal级别的错误信息
        /// </summary>
        /// <param name="message">消息对象</param>
        /// <param name="exception">异常对象</param>
        public void Fatal(object message, Exception exception)
        {
            Fatal(message.ToString(), exception);
        }

        /// <summary>
        /// 以指定格式记录Fatal级别的错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="arg0"></param>
        public void FatalFormat(string format, object arg0)
        {
            Fatal(string.Format(format, arg0));
        }

        /// <summary>
        /// 以指定格式记录Fatal级别的错误信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void FatalFormat(string format, params object[] args)
        {
            Fatal(string.Format(format, args));
        }

        /// <summary>
        /// 以指定格式记录Fatal级别的错误信息
        /// </summary>
        /// <param name="provider">格式化提供对象</param>
        /// <param name="format">格式</param>
        /// <param name="args">格式化参数</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Fatal(string.Format(provider, format, args));
        }

        /// <summary>
        /// 以指定格式记录Fatal级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            Fatal(string.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 以指定格式记录Fatal级别的错误信息
        /// </summary>
        /// <param name="format">格式</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Fatal(string.Format(format, arg0, arg1, arg2));
        }
        #endregion
        #endregion

        #region 私有方法

        /// <summary>
        /// 记录Debug级别的错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        private void Debug(string message, Exception exception)
        {
            AppendLog(message, exception, LogLevel.DEBUG);
        }

        /// <summary>
        /// 记录Info级别的错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        private void Info(string message, Exception exception)
        {
            AppendLog(message, exception, LogLevel.INFO);
        }

        /// <summary>
        /// 记录Warn级别的错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        private void Warn(string message, Exception exception)
        {
            AppendLog(message, exception, LogLevel.WARN);
        }

        /// <summary>
        /// 记录Error级别的错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        private void Error(string message, Exception exception)
        {
            AppendLog(message, exception, LogLevel.ERROR);
        }

        /// <summary>
        /// 记录Fatal级别的错误信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        private void Fatal(string message, Exception exception)
        {
            AppendLog(message, exception, LogLevel.FATAL);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常信息</param>
        /// <param name="level">日志级别</param>
        private void AppendLog(string message, Exception exception, LogLevel level)
        {
            LogManager.Append(logger, message, exception, level);
        }

        #endregion
    }
}
