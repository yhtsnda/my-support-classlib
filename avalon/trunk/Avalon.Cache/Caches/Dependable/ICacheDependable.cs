using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    public interface ICacheDependProvider
    {
        IList<CacheDependResult> GetBatchDependResult(IEnumerable<string> keys);

        void InitSetting(IEnumerable<SettingNode> nodes);

        void Init();
    }


    public class CacheDependResult
    {
        public CacheDependResult(string key, long timestamp)
        {
            Key = key;
            Timestamp = timestamp;
        }

        public CacheDependResult(string key)
        {
            Key = key;
            IsMissing = true;
        }

        public string Key { get; private set; }

        public bool IsMissing { get; private set; }

        public long Timestamp { get; private set; }

        public bool Valid(IQueryTimestamp data)
        {
            return !IsMissing && data.Timestamp > Timestamp;
        }
    }
}
