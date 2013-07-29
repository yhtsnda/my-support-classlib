using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Projects.Tool.Diagnostics
{
    public class ProfilerItem
    {
        List<ProfilerItem> items;

        internal ProfilerItem(string message)
        {
            Start = ProfilerContext.Current.Elapsed;
            Message = message;
            items = new List<ProfilerItem>();
        }
        /// <summary>
        /// 获取 ProfilerContext 开启监视以来，当前实例创建的时间点（毫秒）
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// 获取当前实例历时（毫秒）
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// 获取或设置消息
        /// </summary>
        public string Message { get; set; }

        public string StackTrace { get; set; }
        /// <summary>
        /// 内嵌的项目集合
        /// </summary>
        public List<ProfilerItem> Items
        {
            get { return items; }
        }

        internal void Dispose()
        {
            Duration = (int)ProfilerContext.Current.Elapsed - Start;
        }
    }
}
