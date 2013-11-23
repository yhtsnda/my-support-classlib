using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Profiler
{
    internal class ProfilerServiceData
    {
        public ProfilerServiceData()
        {
            BufferDatas = new ConcurrentQueue<ProfilerData>();
            RequestDatas = new ConcurrentQueue<RequestData>();
            Setting = new ProfilerSetting();
        }

        public ProfilerSetting Setting { get; set; }

        public bool UserProfileEnabled
        {
            get { return ProfilerService.UserProfileEnabled; }
        }

        public ConcurrentQueue<ProfilerData> BufferDatas { get; set; }

        public ConcurrentQueue<RequestData> RequestDatas { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("id             {0}\r\n", Setting.Id);
            sb.AppendFormat("enable         {0}\r\n", Setting.Enabled);
            sb.AppendFormat("mode           {0}\r\n", Setting.Mode);
            sb.AppendFormat("show detail    {0}\r\n", Setting.ShowDetail);
            sb.AppendFormat("static count   {0}\r\n", Setting.StaticCount);
            sb.AppendFormat("url filter     {0}\r\n", Setting.UrlFilter);
            sb.AppendFormat("url no filter  {0}\r\n", Setting.NoUrlFilter);
            sb.AppendFormat("ip filter      {0}\r\n", Setting.IPFilter);
            sb.AppendFormat("request        {0}\r\n", Setting.RequestEnabled);
            sb.AppendFormat("request count  {0}\r\n", Setting.RequestCount);
            sb.AppendFormat("slow           {0}\r\n", Setting.SlowEnabled);
            sb.AppendFormat("slow msec      {0}\r\n", Setting.SlowMilliSecond);
            sb.AppendFormat("slow db        {0}\r\n", Setting.SlowDbCount);
            sb.AppendFormat("slow cache     {0}\r\n", Setting.SlowCacheCount);
            return sb.ToString();
        }
    }
}
