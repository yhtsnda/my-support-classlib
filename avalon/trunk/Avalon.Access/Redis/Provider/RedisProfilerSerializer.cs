using Avalon.Access;
using Avalon.Profiler;
using Avalon.Utility;
using ServiceStack.Redis;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.RedisProvider
{
    public class RedisProfilerSerializer : IProfilerSerializer
    {
        string connName;
        public void Init(SettingNode node)
        {
            connName = node.TryGetValue("connectionName");
            if (String.IsNullOrEmpty(connName))
                throw new MissConfigurationException(node, "connectionName");
        }

        public List<ProfilerData> Load(DateTime date, int index, int length)
        {
            var key = String.Format("profiler:{0}:{1:yyyyMMdd}", SettingProvider.SiteIdentity, date);

            List<string> jsons;
            using (var client = CreateRedisClient())
            {
                jsons = client.GetRangeFromList(key, index, index + length - 1);
            }

            return jsons.Select(o => (ProfilerData)JsonSerializer.DeserializeFromString(o, typeof(ProfilerData))).ToList();
        }

        public void Save(ProfilerData data)
        {
            var key = String.Format("profiler:{0}:{1:yyyyMMdd}", SettingProvider.SiteIdentity, data.RequestTime);
            using (var client = CreateRedisClient())
            {
                var json = JsonSerializer.SerializeToString(data);
                client.PushItemToList(key, json);
            }
        }

        IRedisClient CreateRedisClient()
        {
            return RedisManager.Instance.CreateRedisClient(connName);
        }
    }
}
