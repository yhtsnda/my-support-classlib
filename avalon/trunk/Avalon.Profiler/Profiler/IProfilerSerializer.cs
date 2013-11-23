using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Utility;

namespace Avalon.Profiler
{
    public interface IProfilerSerializer
    {
        void Init(SettingNode node);

        List<ProfilerData> Load(DateTime date, int index, int length);

        void Save(ProfilerData data);
    }

    internal class DefaultProfilerSerializer : IProfilerSerializer
    {
        List<ProfilerData> datas = new List<ProfilerData>();

        public void Init(SettingNode node)
        {
        }

        public List<ProfilerData> Load(DateTime date, int index, int length)
        {
            date = date.Date;
            return datas.Where(o => o.RequestTime.Date == date).Skip(index).Take(length).ToList();
        }

        public void Save(ProfilerData data)
        {
            datas.Add(data);
        }
    }
}
