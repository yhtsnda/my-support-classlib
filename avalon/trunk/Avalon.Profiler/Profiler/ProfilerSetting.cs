using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public class ProfilerSetting : ISetting
    {
        public ProfilerSetting()
        {
            Mode = ProfilerMode.Request;
            StaticCount = 20;
            RequestCount = 20;
            Id = "profiler:" + SettingProvider.SiteIdentity;
            SlowMilliSecond = 1000;
            SlowDbCount = 10;
            SlowCacheCount = 20;
        }

        /// <summary>
        /// 键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 是否记录请求信息
        /// </summary>
        public bool RequestEnabled { get; set; }

        /// <summary>
        /// 记录请求的最大记录数
        /// </summary>
        public int RequestCount { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public ProfilerMode Mode { get; set; }

        /// <summary>
        /// 是否显示详细堆栈
        /// </summary>
        public bool ShowDetail { get; set; }

        /// <summary>
        /// 静态模式的最大记录数
        /// </summary>
        public int StaticCount { get; set; }

        /// <summary>
        /// 匹配的URL
        /// </summary>
        public string UrlFilter { get; set; }

        /// <summary>
        /// 过滤的URL
        /// </summary>
        public string NoUrlFilter { get; set; }

        /// <summary>
        /// 匹配的IP
        /// </summary>
        public string IPFilter { get; set; }

        /// <summary>
        /// 是否进行慢处理记录
        /// </summary>
        public bool SlowEnabled { get; set; }

        /// <summary>
        /// 记录为慢处理的处理毫秒数
        /// </summary>
        public int SlowMilliSecond { get; set; }

        /// <summary>
        /// 记录为慢处理的nhibernate数量
        /// </summary>
        public int SlowDbCount { get; set; }

        /// <summary>
        /// 记录为慢处理的外部缓存的数量
        /// </summary>
        public int SlowCacheCount { get; set; }

        public bool IsSlow(ProfilerData data)
        {
            return SlowEnabled && (
                SlowMilliSecond > 0 && data.Duration > SlowMilliSecond ||
                SlowDbCount > 0 && data.Traces.Count(o => o.Type == "nhibernate" || o.Type == "mongo") > SlowDbCount ||
                SlowCacheCount > 0 && data.Traces.Count(o => o.Type == "remotecache") > SlowCacheCount
                );
        }
    }
}
