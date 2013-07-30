using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projects.Tool.Diagnostics
{
    public class WatchItem
    {
        List<WatchItem> items;

        internal WatchItem(string message)
        {
            Start = ProfilerContext.Current.Elapsed;
            Message = message;
            items = new List<WatchItem>();
        }

        /// <summary>
        /// 获取 ProfilerContext 开启监视以来，当前实例创建的时间点（毫秒）
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 获取当前实例历时（毫秒）
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// 获取或设置消息
        /// </summary>
        public string Message { get; set; }

        public string StackTrace { get; set; }

        /// <summary>
        /// 内嵌的项目集合
        /// </summary>
        public List<WatchItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        internal void Dispose()
        {
            Duration = (int)ProfilerContext.Current.Elapsed - Start;
        }
    }
}
