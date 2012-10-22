using System;

namespace Projects.Tool.WeblogProvider
{
    /// <summary>
    /// 日志对象
    /// </summary>
    internal class LogEntity
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// 日志记录者
        /// </summary>
        public string Logger { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 日志上下文
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 日志创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 日志上下文对象
    /// </summary>
    [Serializable]
    internal class StackTrace
    {
        /// <summary>
        /// URI 的绝对路径
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// 客户端上次请求的 URL 的信息
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// URL参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 表单参数
        /// </summary>
        public string Form { get; set; }

        /// <summary>
        /// 用户标识对象
        /// </summary>
        public UserIdentity User { get; set; }

        /// <summary>
        /// 用户标识对象
        /// </summary>
        [Serializable]
        public class UserIdentity
        {
            /// <summary>
            /// 用户名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 是否有权限
            /// </summary>
            public bool IsAuthenticated { get; set; }
        }
    }
}
