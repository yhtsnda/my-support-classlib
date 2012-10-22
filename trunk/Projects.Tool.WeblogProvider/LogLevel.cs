using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.WeblogProvider
{
    /// <summary>
    /// 日志级别
    /// 优先级从高到低依次排列如下：FATAL，ERROR，WARN，INFO，DEBUG
    /// </summary>
    internal enum LogLevel : byte
    {
        /// <summary>
        /// 程序内部的信息，用于调试
        /// </summary>
        DEBUG = 1,

        /// <summary>
        /// 强调应用的执行的进度，关键分支的记录，指明程序是否符合正确的业务逻辑
        /// </summary>
        INFO = 2,

        /// <summary>
        /// 潜在的有害状态
        /// </summary>
        WARN = 3,

        /// <summary>
        /// 错误事件发生，程序或许依然能够运行
        /// </summary>
        ERROR = 4,

        /// <summary>
        /// 错误可能会导致应用崩溃
        /// </summary>
        FATAL = 5
    }
}
